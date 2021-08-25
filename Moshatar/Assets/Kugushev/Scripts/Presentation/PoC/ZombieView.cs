using System;
using System.Collections;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Presentation.PoC.Music;
using UnityEngine;
using Zenject;
using UniRx;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class ZombieView : MonoBehaviour, IPoolable<Vector3, ZombiesSpawner, IMemoryPool>
    {
        private const float PursueTime = 3;
        private const float DeathTime = 2f;
        private const float HitMultiplier = 10f;
        public const float AttackDistance = 1.3f;
        private const int MaxHitPoints = 20;
        private const int HardHitPower = 10;
        private const int MaxOneHitDamage = 20;
        private const int BleedDamage = 5;
        private const int AttackMinCooldownSeconds = 10;
        private const int AttackMaxCooldownSeconds = 120;

        private static readonly int HitReactionParameter = Animator.StringToHash("HitReaction");
        private static readonly int AttackParameter = Animator.StringToHash("Attack");
        private static readonly int KnockedDownState = Animator.StringToHash("Knocked Down");
        private static readonly int RunningState = Animator.StringToHash("Zombie Running");
        private static readonly int IdleState = Animator.StringToHash("Zombie Idle");

        [SerializeField] private AudioSource riseEffect;
        [SerializeField] private AudioSource hitEffect;
        [SerializeField] private AudioSource attackEffect;
        [SerializeField] private SimpleHealthBar healthBar;
        [SerializeField] private Transform projectingPosition;
        [SerializeField] private GameObject selectedForChargeMarker;
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private HitRecorder[] hitRecorders;

        [Inject] private Hero _hero;
        [Inject] private PopupText.Factory _popupTextFactory;
        [Inject] private ZombieProjectile.Factory _projectilesFactory;
        [Inject] private GameDirector _director;
        [Inject] private ChargeManager _chargeManager;

        private Vector3 _start;
        private ZombiesSpawner _spawner;
        private IMemoryPool _memoryPool;

        private DateTime? _deathTime;
        private readonly ReactiveProperty<int> _hitPoints = new ReactiveProperty<int>();

        private readonly List<Fist> _fistsBuffer = new List<Fist>(1);
        private readonly List<Weapon> _weaponsBuffer = new List<Weapon>(1);
        private Animator _animator;
        private Rigidbody _rigidbody;
        private bool _dead;

        // todo: ugly hack, make something reusable
        private bool _isBleeding;
        private float _bleedingTicks = 0f;

        private readonly ReactiveProperty<bool> _pursuing = new ReactiveProperty<bool>(false);
        private float _movingTime;

        private DateTime _lastAttack = DateTime.Now;


        private Vector3 HeroPositionOnSurface
        {
            get
            {
                var position = _hero.HeadPosition;
                position.y = 0;
                return position;
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            // _pursuing.Subscribe(PursuingChanged).AddTo(this);
            _hitPoints.Subscribe(HitPointsChanges).AddTo(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_dead)
                return;

            var hitPosition = other.transform.position;

            int damage;
            if (TryGetFist(other, out var fist))
            {
                int power = GetPower(fist.Velocity);
                damage = fist.RegisterFistHit(IsHardHit(power));
            }
            else if (TryGetWeapon(other, out var weapon))
            {
                int power = GetPower(weapon.Velocity);
                damage = weapon.RegisterWeaponHit(IsHardHit(power), power);

                if (IsHardHit(power))
                    FindHitRecorder(other).RegisterHitEnter(hitPosition, damage);
            }
            else
            {
                Debug.LogError("No weapon or fist");
                return;
            }

            Suffer(damage, hitPosition);

            int GetPower(float velocity) => Mathf.FloorToInt(velocity * 100);
            bool IsHardHit(int power) => power > HardHitPower;
        }

        private void OnTriggerExit(Collider other)
        {
            if (TryGetWeapon(other, out var weapon))
            {
                var direction = FindHitRecorder(other).RegisterHitExit(other.transform.position);
                if (direction != AttackDirection.None)
                {
                    var combo = weapon.RegisterWeaponHitFinished(direction);
                    ProcessCombo(combo, other.transform.position);
                }
            }
        }

        public void ChooseForCharge()
        {
            selectedForChargeMarker.SetActive(true);
            _chargeManager.ActiveTarget = this;
        }

        public void DeselectForCharge()
        {
            selectedForChargeMarker.SetActive(false);
        }

        public void UnChooseForCharge()
        {
        }

        private void ProcessCombo(Combo combo, Vector3 hitPoint)
        {
            Suffer(combo.Damage, hitPoint, isCombo: true);

            switch (combo.Effect)
            {
                case DamageEffect.Bleed:
                    _isBleeding = true;
                    _bleedingTicks = 0f;
                    _popupTextFactory.Create("BLEED", hitPoint);
                    break;
                case DamageEffect.Purge:
                    _isBleeding = false;
                    _popupTextFactory.Create("PURGE", hitPoint);
                    break;
            }
        }

        private bool TryGetFist(Collider other, out Fist fist)
        {
            _fistsBuffer.Clear();
            other.GetComponents(_fistsBuffer);

            if (_fistsBuffer.Count != 1)
            {
                fist = null;
                return false;
            }

            fist = _fistsBuffer[0];
            return true;
        }

        private bool TryGetWeapon(Collider other, out Weapon weapon)
        {
            _weaponsBuffer.Clear();
            other.GetComponents(_weaponsBuffer);

            if (_weaponsBuffer.Count != 1)
            {
                weapon = null;
                return false;
            }

            weapon = _weaponsBuffer[0];
            return true;
        }

        private HitRecorder FindHitRecorder(Collider other)
        {
            for (var i = 0; i < hitRecorders.Length; i++)
            {
                var hitRecorder = hitRecorders[i];
                if (other.CompareTag(hitRecorder.WeaponTag))
                {
                    return hitRecorder;
                }
            }

            throw new Exception($"No hit recorders for {other.tag}");
        }

        private void Suffer(int damage, Vector3 hitPoint, bool ignoreReaction = false, bool isCombo = false)
        {
            if (damage <= 0)
                return;

            if (!isCombo)
            {
                // limit damage
                damage = Math.Min(damage, MaxOneHitDamage);
            }

            _pursuing.Value = false;

            _hitPoints.Value -= damage;

            if (!ignoreReaction)
            {
                _animator.SetTrigger(HitReactionParameter);
                hitEffect.Play();
            }

            _popupTextFactory.Create(StringBag.FromInt(damage), hitPoint);

            if (_hitPoints.Value <= 0)
            {
                Die(damage);
                if (_director.IsBit)
                {
                    // todo: spawn coin
                    var pos = transform.position;
                    pos.y = 1.3f;
                    Instantiate(coinPrefab, pos, Quaternion.Euler(-90, 0, 0));
                }
            }
        }

        void IPoolable<Vector3, ZombiesSpawner, IMemoryPool>.OnSpawned(Vector3 p1, ZombiesSpawner p2, IMemoryPool p3)
        {
            _rigidbody.velocity = Vector3.zero;

            transform.position = p1;
            transform.LookAt(HeroPositionOnSurface);

            _spawner = p2;
            _spawner.Set(this);

            _memoryPool = p3;
            // _pursuing.Value = true;
            _movingTime = 0;

            _deathTime = null;
            _isBleeding = false;

            _start = p1;
            _hitPoints.Value = MaxHitPoints;

            _lastAttack = DateTime.Now;

            riseEffect.Play();
            _dead = false;
        }


        void IPoolable<Vector3, ZombiesSpawner, IMemoryPool>.OnDespawned()
        {
            _spawner.Release(this);
            _memoryPool = null;
            _spawner = null;
            _deathTime = null;
            _isBleeding = false;
            _rigidbody.velocity = Vector3.zero;
        }

        private void Die(int damage)
        {
            _dead = true;

            _deathTime = DateTime.Now;

            var force = (transform.position - Vector3.zero).normalized;
            force.y += 0.5f;
            force *= damage * HitMultiplier;
            _rigidbody.AddForce(force);

            _animator.Play(KnockedDownState);
        }

        private void Update()
        {
            if (_dead)
                return;

            transform.LookAt(HeroPositionOnSurface);

            if (_pursuing.Value)
            {
                _movingTime += Time.deltaTime;
                var p = Vector3.Lerp(_start, HeroPositionOnSurface, _movingTime / PursueTime);

                if (Vector3.Distance(p, HeroPositionOnSurface) < AttackDistance)
                    _pursuing.Value = false;

                transform.position = p;
            }

            if (_isBleeding)
            {
                _bleedingTicks += Time.deltaTime;
                if (_bleedingTicks > 1f)
                {
                    _bleedingTicks = 0f;
                    Suffer(BleedDamage, transform.position, true);
                }
            }

            if (_deathTime != null && DateTime.Now - _deathTime > TimeSpan.FromSeconds(DeathTime))
            {
                _memoryPool.Despawn(this);
                return;
            }

            int attackCooldownSeconds = Random.Range(AttackMinCooldownSeconds, AttackMaxCooldownSeconds);
            if (_director.CurrentSectionType == SongSectionType.Battle &&
                (DateTime.Now - _lastAttack).TotalSeconds > attackCooldownSeconds)
            {
                Attack();

                _lastAttack = DateTime.Now;
            }
        }

        private void Attack()
        {
            _animator.SetTrigger(AttackParameter);
            attackEffect.Play();

            _projectilesFactory.Create(projectingPosition.position, _hero.HeadPosition);
        }

        private void PursuingChanged(bool pursuing)
        {
            _animator.Play(pursuing ? RunningState : IdleState);
        }

        private void HitPointsChanges(int hitPoints)
        {
            healthBar.UpdateBar(hitPoints, MaxHitPoints);
        }

        public class Factory : PlaceholderFactory<Vector3, ZombiesSpawner, ZombieView>
        {
        }
    }
}