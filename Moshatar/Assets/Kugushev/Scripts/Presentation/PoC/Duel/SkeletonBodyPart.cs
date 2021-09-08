using System;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class SkeletonBodyPart : MonoBehaviour
    {
        private Skeleton _parent;

        public void Init(Skeleton parent)
        {
            _parent = parent;
        }

        private void OnTriggerEnter(Collider other)
        {
            _parent.RegisterHit(other.CompareTag("BigProjectile"));
        }
    }
}