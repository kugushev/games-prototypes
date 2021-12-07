using System;
using System.Collections;
using Kugushev.Scripts.Battle.Core;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter
    {
        private EnemyFighter _model;
        private static readonly int AnimationSpeedv = Animator.StringToHash("speedv");
        private static readonly int AnimationAttack1H1 = Animator.StringToHash("Attack1h1");
        private static readonly int AnimationHit1 = Animator.StringToHash("Hit1");
        private static readonly int AnimationFall1 = Animator.StringToHash("Fall1");
        private readonly WaitForSeconds _waitToDie = new WaitForSeconds(3);
        private readonly WaitForSeconds _waitForDamage = new WaitForSeconds(0.5f);
        private bool _damaging;

        public override BaseFighter Model => _model ?? throw new PropertyIsNotInitializedException();

        public void Init(EnemyFighter model)
        {
            _model = model;
            if (_model.IsBig)
                transform.localScale *= 2f;
        }

        protected override void OnStart()
        {
            StartCoroutine(HandleDot());
        }

        private IEnumerator HandleDot()
        {
            while (true)
            {
                yield return _waitForDamage;
                if (_damaging)
                    _model.Suffer(BattleConstants.FireBreathDamage);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SmallProjectile"))
                _model.Suffer(BattleConstants.HeroDamage);
            else if (other.CompareTag("BigProjectile"))
                _model.Suffer(BattleConstants.HeroDamageSuper);

            if (other.CompareTag("Dot"))
                _damaging = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Dot"))
                _damaging = false;
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
            StartCoroutine(Destroying());
        }

        private IEnumerator Destroying()
        {
            // todo: move under ground
            yield return _waitToDie;
            Destroy(gameObject);
        }
    }
}