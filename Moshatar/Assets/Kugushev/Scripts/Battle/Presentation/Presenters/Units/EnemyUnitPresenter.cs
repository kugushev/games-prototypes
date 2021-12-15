using System;
using System.Collections;
using Kugushev.Scripts.Battle.Core;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter, IPoolable<Vector3, EnemyFighter, IMemoryPool>
    {
        [SerializeField] private Collider attackCollider;
        [SerializeField] private AudioSource attackSound;
        [SerializeField] private float attackAnimationShift = -15f;

        [Inject] private readonly HeroUnit _heroUnit;
        [Inject] private BattleGameplayManager _gameplayManager;

        private static readonly int AnimationSpeedv = Animator.StringToHash("speedv");
        private static readonly int AnimationAttack1H1 = Animator.StringToHash("Attack1h1");
        private static readonly int AnimationHit1 = Animator.StringToHash("Hit1");
        private static readonly int AnimationFall1 = Animator.StringToHash("Fall1");
        private readonly WaitForSeconds _waitToDie = new WaitForSeconds(1f);
        private readonly WaitForSeconds _waitForDamage = new WaitForSeconds(0.5f);
        private IMemoryPool _memoryPool;

        public EnemyFighter Model { get; private set; }

        public void OnSpawned(Vector3 p1, EnemyFighter p2, IMemoryPool pool)
        {
            _memoryPool = pool;

            var t = transform;

            t.position = p1;
            Model = p2;

            if (Model.IsBig)
                t.localScale = DefaultScale * 2f;

            OnModelSet(Model);

            attackCollider.enabled = true;
        }

        public void OnDespawned()
        {
            _memoryPool = null;

            OnModelRemoved(Model);
            Model = null;

            transform.localScale = DefaultScale;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (Model == null)
                return;

            if (other.CompareTag("SmallProjectile"))
            {
                Model.Suffer(_gameplayManager.Parameters.HeroDamage);
                _heroUnit.Model.Lifesteal();
            }
            else if (other.CompareTag("BigProjectile"))
            {
                Model.Suffer(_gameplayManager.Parameters.HeroDamageSuper);
                _heroUnit.Model.Lifesteal();
            }
            else if (other.CompareTag("Dot"))
            {
                Model.Burning = true;
                _heroUnit.Model.Lifesteal();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Model == null)
                return;

            if (other.CompareTag("Dot"))
                Model.Burning = true;
        }

        protected override void OnActivityChanged(ActivityType newActivityType)
        {
            var speed = newActivityType switch
            {
                ActivityType.Stay => 0f,
                ActivityType.Move => 1f,
                _ => 0f
            };
            Animator.SetFloat(AnimationSpeedv, speed);
        }

        protected override void OnAttacking(BaseFighter target)
        {
            if (target is HeroFighter hero)
            {
                var t = transform;
                var lookPos = hero.HeadPosition - t.position;
                lookPos.y = 0f;
                var rotation = Quaternion.LookRotation(lookPos);
                var euler = rotation.eulerAngles;
                euler.y += attackAnimationShift;
                t.rotation = Quaternion.Euler(euler);

                attackSound.Play();
            }

            Animator.SetTrigger(AnimationAttack1H1);
        }

        protected override void OnHurt() => Animator.SetTrigger(AnimationHit1);

        protected override void OnDie()
        {
            Animator.SetTrigger(AnimationFall1);
            attackCollider.enabled = false;
            StartCoroutine(Destroying());
        }

        private IEnumerator Destroying()
        {
            yield return _waitToDie;
            _memoryPool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector3, EnemyFighter, EnemyUnitPresenter>
        {
        }
    }
}