﻿using System;
using System.Collections;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class ZombieView : MonoBehaviour, IPoolable<Vector3, IMemoryPool>
    {
        private const float PursueTime = 3;
        private const float DeathTime = 2f;
        private const float HitMultiplier = 10f;
        private const float AttackDistance = 1.3f;
        private const int MaxHitPoints = 100000000;
        private static readonly Vector3 HeroPosition = Vector3.zero;

        private static readonly int HitReactionParameter = Animator.StringToHash("HitReaction");
        private static readonly int KnockedDownState = Animator.StringToHash("Knocked Down");
        private static readonly int RunningState = Animator.StringToHash("Zombie Running");
        private static readonly int IdleState = Animator.StringToHash("Zombie Idle");

        [SerializeField] private AudioSource hitEffect;
        [SerializeField] private SimpleHealthBar healthBar;
        [SerializeField] private HitRecorder[] hitRecorders;

        [Inject] private PopupText.Factory _popupTextFactory;

        private IMemoryPool _memoryPool;
        private Vector3 _start;
        private DateTime? _deathTime;
        private readonly ReactiveProperty<int> _hitPoints = new ReactiveProperty<int>();

        private readonly List<Fist> _fistsBuffer = new List<Fist>(1);
        private Animator _animator;
        private Rigidbody _rigidbody;

        private readonly ReactiveProperty<bool> _pursuing = new ReactiveProperty<bool>();
        private float _movingTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            _pursuing.Subscribe(PursuingChanged).AddTo(this);
            _hitPoints.Subscribe(HitPointsChanges).AddTo(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            _fistsBuffer.Clear();
            other.GetComponents(_fistsBuffer);

            if (_fistsBuffer.Count != 1)
            {
                Debug.LogError($"Unexpected buffers count {_fistsBuffer.Count}");
                return;
            }

            var fist = _fistsBuffer[0];
            int damage = Mathf.FloorToInt(fist.Velocity * 100);

            RecordHit(other, damage, (h, p, d) => h.RegisterHitEnter(p, d));

            Suffer(damage, fist.transform.position);

            _fistsBuffer.Clear();
        }

        private void OnTriggerExit(Collider other)
        {
            RecordHit(other, 0, (h, p, d) => h.RegisterHitExit(p));
        }

        private void RecordHit(Collider other, int damage, Action<HitRecorder, Vector3, int> action)
        {
            for (var i = 0; i < hitRecorders.Length; i++)
            {
                var hitRecorder = hitRecorders[i];
                if (other.CompareTag(hitRecorder.WeaponTag))
                {
                    action(hitRecorder, other.gameObject.transform.position, damage);
                    break;
                }
            }
        }

        private void Suffer(int damage, Vector3 hitPoint)
        {
            _pursuing.Value = false;

            if (damage > 20)
                damage *= 2;

            Suffered(damage, hitPoint);

            //if (damage > 20)
            // todo: recognize hard kick
            if (_hitPoints.Value <= 0)
                Die(damage);
        }

        void IPoolable<Vector3, IMemoryPool>.OnSpawned(Vector3 p1, IMemoryPool p2)
        {
            _rigidbody.velocity = Vector3.zero;

            transform.position = p1;
            transform.LookAt(HeroPosition);

            _memoryPool = p2;
            _pursuing.Value = true;
            _movingTime = 0;

            _deathTime = null;

            _start = p1;
            _hitPoints.Value = MaxHitPoints;
        }


        void IPoolable<Vector3, IMemoryPool>.OnDespawned()
        {
            _memoryPool = null;
            _deathTime = null;
            _rigidbody.velocity = Vector3.zero;
        }

        private void Suffered(int damage, Vector3 hitPoint)
        {
            _hitPoints.Value -= damage;

            _animator.SetTrigger(HitReactionParameter);
            hitEffect.Play();

           // _popupTextFactory.Create(StringBag.FromInt(damage), hitPoint);
        }

        private void Die(int damage)
        {
            _deathTime = DateTime.Now;

            var force = (transform.position - Vector3.zero).normalized;
            force.y += 0.5f;
            force *= damage * HitMultiplier;
            _rigidbody.AddForce(force);

            _animator.Play(KnockedDownState);
        }

        private void Update()
        {
            if (_pursuing.Value)
            {
                _movingTime += Time.deltaTime;
                var p = Vector3.Lerp(_start, HeroPosition, _movingTime / PursueTime);

                if (Vector3.Distance(p, HeroPosition) < AttackDistance)
                    _pursuing.Value = false;

                transform.position = p;
            }

            if (_deathTime != null && DateTime.Now - _deathTime > TimeSpan.FromSeconds(DeathTime))
                _memoryPool.Despawn(this);
        }

        private void PursuingChanged(bool pursuing)
        {
            _animator.Play(pursuing ? RunningState : IdleState);
        }

        private void HitPointsChanges(int hitPoints)
        {
            healthBar.UpdateBar(hitPoints, MaxHitPoints);
        }

        public class Factory : PlaceholderFactory<Vector3, ZombieView>
        {
        }
    }
}