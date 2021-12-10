using System;
using System.Collections;
using Kugushev.Scripts.Battle.Core;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter, IPoolable<Vector3, EnemyFighter, IMemoryPool>
    {
        [SerializeField] private Collider attackCollider;

        private EnemyFighter _model;
        private static readonly int AnimationSpeedv = Animator.StringToHash("speedv");
        private static readonly int AnimationAttack1H1 = Animator.StringToHash("Attack1h1");
        private static readonly int AnimationHit1 = Animator.StringToHash("Hit1");
        private static readonly int AnimationFall1 = Animator.StringToHash("Fall1");
        private readonly WaitForSeconds _waitToDie = new WaitForSeconds(1f);
        private readonly WaitForSeconds _waitForDamage = new WaitForSeconds(0.5f);
        private IMemoryPool _memoryPool;

        public void OnSpawned(Vector3 p1, EnemyFighter p2, IMemoryPool pool)
        {
            _memoryPool = pool;

            var t = transform;

            t.position = p1;
            _model = p2;

            if (_model.IsBig)
                t.localScale = DefaultScale * 2f;

            OnModelSet(_model);

            attackCollider.enabled = true;
        }

        public void OnDespawned()
        {
            _memoryPool = null;

            OnModelRemoved(_model);
            _model = null;

            transform.localScale = DefaultScale;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (_model == null)
                return;

            if (other.CompareTag("SmallProjectile"))
                _model.Suffer(BattleConstants.HeroDamage);
            else if (other.CompareTag("BigProjectile"))
                _model.Suffer(BattleConstants.HeroDamageSuper);
            else if (other.CompareTag("Dot"))
                _model.Burning = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_model == null)
                return;

            if (other.CompareTag("Dot"))
                _model.Burning = true;
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

        protected override void OnAttacking() => Animator.SetTrigger(AnimationAttack1H1);

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