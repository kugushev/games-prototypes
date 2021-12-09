using System;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class PlayerUnitPresenter : BaseUnitPresenter
    {
        private PlayerFighter _model;
        private static readonly int AnimationSpeed = Animator.StringToHash("speed");
        private static readonly int AnimationAttack = Animator.StringToHash("attack");
        private static readonly int AnimationHit = Animator.StringToHash("hit");
        private static readonly int AnimationDie = Animator.StringToHash("die");

        public void Init(PlayerFighter model)
        {
            _model = model;
            OnModelSet(_model);
        }

        private void OnDestroy()
        {
            OnModelRemoved(_model);
        }

        protected override void OnActivityChanged(ActivityType newActivityType) =>
            Animator.SetFloat(AnimationSpeed, newActivityType switch
            {
                ActivityType.Stay => 0f,
                ActivityType.Move => 1f,
                _ => 0f // todo: log error
            });

        protected override void OnAttacking() => Animator.SetTrigger(AnimationAttack);

        protected override void OnHurt() => Animator.SetTrigger(AnimationHit);

        protected override void OnDie() => Animator.SetTrigger(AnimationDie);
    }
}