using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Presentation.Presenters.Squad;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters
{
    public class HeroHeadPresenter : MonoBehaviour
    {
        [SerializeField] private VolumeProfile volumeProfile;

        [Inject] private readonly HeroUnit _heroUnit;

        private Vignette _vignette;
        private readonly List<EnemyWeapon> _enemyWeponsBuffer = new List<EnemyWeapon>(1);

        private void Start()
        {
            if (!volumeProfile.TryGet(out _vignette))
                Debug.LogError("No Vignette found");

            _heroUnit.Hurt += HpChanged;
        }

        private void Update()
        {
            if (_heroUnit.Model != null) 
                _heroUnit.Model.HeadPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyProjectile"))
            {
                // print("Hit");
                _enemyWeponsBuffer.Clear();
                other.GetComponents(_enemyWeponsBuffer);

                var enemy = _enemyWeponsBuffer.Single();

                var damage = enemy.Owner.Model.Character.Damage;
                _heroUnit.Model.Suffer(damage);

                _enemyWeponsBuffer.Clear();
            }
        }

        private void HpChanged(int value, int max)
        {
            if (_vignette != null && _vignette.intensity != null)
                _vignette.intensity.value = 1f - (float)value / max;
        }

        private void OnDestroy()
        {
            _heroUnit.Hurt -= HpChanged;

            if (_vignette != null && _vignette.intensity != null)
                _vignette.intensity.value = 0;
        }
    }
}