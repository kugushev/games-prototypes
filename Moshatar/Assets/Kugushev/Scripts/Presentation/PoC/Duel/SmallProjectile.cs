using System;
using Kugushev.Scripts.Presentation.PoC.Common;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class SmallProjectile: Projectile
    {
        public class Factory : PlaceholderFactory<Vector3, Vector3, float, SmallProjectile>
        {
        }


        private void OnTriggerEnter(Collider other)
        {
            Despawn();
        }
    }
}