using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    [RequireComponent(typeof(Animator))]
    public class Skeleton : MonoBehaviour
    {
        private float _hitPoints = 100f;
        private static readonly int Hit1 = Animator.StringToHash("Hit1");
        private static readonly int Fall1 = Animator.StringToHash("Fall1");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var bodyParts = GetComponentsInChildren<SkeletonBodyPart>();
            foreach (var bodyPart in bodyParts)
                bodyPart.Init(this);
        }

        public void RegisterHit(bool hard)
        {
            if (_hitPoints < 0)
                return;

            _hitPoints -= hard ? 100 : Random.Range(10, 25);

            _animator.SetTrigger(_hitPoints > 0 ? Hit1 : Fall1);
        }
    }
}