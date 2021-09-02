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

        public void RegisterHit()
        {
            if (_hitPoints < 0)
                return;

            _hitPoints -= Random.Range(25, 40);

            _animator.SetTrigger(_hitPoints > 0 ? Hit1 : Fall1);
        }
    }
}