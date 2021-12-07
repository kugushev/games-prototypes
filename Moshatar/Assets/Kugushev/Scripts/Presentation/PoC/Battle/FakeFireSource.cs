using System;
using System.Collections;
using Kugushev.Scripts.Presentation.PoC.Duel;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC.Battle
{
    public class FakeFireSource : MonoBehaviour
    {
        private const float ProjectileSpeed = 10f;
        
        [Inject] private BigProjectile.Factory _playerBigProjectile;
        [Inject] private SmallProjectile.Factory _playerSmallProjectile;

        private readonly WaitForSeconds _shootCooldown = new WaitForSeconds(2f);

        private readonly Vector3[] _launchers =
        {
            new Vector3(40, 1, 30),
            new Vector3(30, 1, -30),
            new Vector3(20, 1, -50),
            new Vector3(-50, 1, 30),
            new Vector3(-30, 1, -30),
        };

        private IEnumerator Start()
        {
            while (true)
            {
                var source = _launchers[Random.Range(0, _launchers.Length)];

                var isBig = Random.Range(0, 5) == 0;
                if (isBig) 
                    _playerBigProjectile.Create(source, Vector3.up, ProjectileSpeed);
                else
                    _playerSmallProjectile.Create(source, Vector3.up, ProjectileSpeed);

                yield return _shootCooldown;
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var launcher in _launchers)
            {
                Gizmos.DrawSphere(launcher, 1f);
            }
        }
    }
}