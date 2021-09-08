using Kugushev.Scripts.Presentation.PoC.Common;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class BigProjectile : Projectile
    {
        public class Factory : PlaceholderFactory<Vector3, Vector3, float, BigProjectile>
        {
        }
    }
}