using System;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter
    {
        private EnemyFighter _model;
        private static readonly int AnimationSpeedv = Animator.StringToHash("speedv");
        private static readonly int AnimationAttack1H1 = Animator.StringToHash("Attack1h1");
        private static readonly int AnimationHit1 = Animator.StringToHash("Hit1");
        private static readonly int AnimationFall1 = Animator.StringToHash("Fall1");
        public override BaseFighter Model => _model ?? throw new PropertyIsNotInitializedException();

        public void Init(EnemyFighter model) => _model = model;

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

        protected override void OnHurt(BaseFighter attacker)=> Animator.SetTrigger(AnimationHit1);

        protected override void OnDie()=> Animator.SetTrigger(AnimationFall1);
    }
}