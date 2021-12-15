using System;
using System.Collections;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class PlayerUnitPresenter : BaseUnitPresenter, IPoolable<Vector3, PlayerFighter, IMemoryPool>
    {
        private PlayerFighter _model;
        private IMemoryPool _memoryPool;
        private static readonly int AnimationSpeed = Animator.StringToHash("speed");
        private static readonly int AnimationAttack = Animator.StringToHash("attack");
        private static readonly int AnimationHit = Animator.StringToHash("hit");
        private static readonly int AnimationDie = Animator.StringToHash("die");
        private readonly WaitForSeconds _waitToDie = new WaitForSeconds(1f);

        public void OnSpawned(Vector3 p1, PlayerFighter p2, IMemoryPool p3)
        {
            _memoryPool = p3;

            var t = transform;

            t.position = p1;
            _model = p2;
            
            OnModelSet(_model);
        }

        public void OnDespawned()
        {
            _memoryPool = null;

            OnModelRemoved(_model);
            _model = null;
        }

        protected override void OnActivityChanged(ActivityType newActivityType) =>
            Animator.SetFloat(AnimationSpeed, newActivityType switch
            {
                ActivityType.Stay => 0f,
                ActivityType.Move => 1f,
                _ => 0f // todo: log error
            });

        protected override void OnAttacking(BaseFighter target) => Animator.SetTrigger(AnimationAttack);

        protected override void OnHurt() => Animator.SetTrigger(AnimationHit);
        
        protected override void OnDie()
        {
            Animator.SetTrigger(AnimationDie);
            StartCoroutine(Destroying());
        }

        private IEnumerator Destroying()
        {
            yield return _waitToDie;
            _memoryPool.Despawn(this);
        }
        
        public class Factory : PlaceholderFactory<Vector3, PlayerFighter, PlayerUnitPresenter>
        {
        }
    }
}