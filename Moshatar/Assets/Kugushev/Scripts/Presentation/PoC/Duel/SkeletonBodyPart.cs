using System;
using System.Collections;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class SkeletonBodyPart : MonoBehaviour
    {
        private readonly WaitForSeconds _waitForDamage = new WaitForSeconds(1);
        
        private Skeleton _parent;
        private bool _damaging;
        
        public void Init(Skeleton parent)
        {
            _parent = parent;
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return _waitForDamage;
                if (_damaging) 
                    _parent.RegisterHit(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BigProjectile")) 
                _parent.RegisterHit(true);
            else if (other.CompareTag("SmallProjectile")) 
                _parent.RegisterHit(false);
            else if (other.CompareTag("Dot")) 
                _damaging = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Dot")) 
                _damaging = false;
        }
    }
}