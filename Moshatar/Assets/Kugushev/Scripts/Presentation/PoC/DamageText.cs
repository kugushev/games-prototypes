using System;
using System.Collections;
using DG.Tweening;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class DamageText : MonoBehaviour, IPoolable<int, Vector3, IMemoryPool>
    {
        private const float Lifetime = 2f;
        private const float TopPosition = 3f;

        private readonly WaitForSeconds _wait = new WaitForSeconds(Lifetime);

        [SerializeField] private TextMeshProUGUI damageText;

        private IMemoryPool _memoryPool;

        void IPoolable<int, Vector3, IMemoryPool>.OnSpawned(int p1, Vector3 p2, IMemoryPool p3)
        {
            damageText.text = StringBag.FromInt(p1);
            transform.position = p2;
            _memoryPool = p3;

            transform.DOMoveY(TopPosition, Lifetime);
            StartCoroutine(CountDownToDepawn());
        }

        void IPoolable<int, Vector3, IMemoryPool>.OnDespawned()
        {
            damageText.text = "Leak";
            _memoryPool = null;
        }


        private IEnumerator CountDownToDepawn()
        {
            yield return _wait;
            _memoryPool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<int, Vector3, DamageText>
        {
        }
    }
}