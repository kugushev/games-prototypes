using System;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class HeroHead : MonoBehaviour
    {
        [Inject] private Hero _hero;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyProjectile"))
            {
                _hero.Suffer();
            }
        }
    }
}