using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class ZombieProjectile : MonoBehaviour, IPoolable<Vector3, Vector3, IMemoryPool>
    {
        private const float LifetimeSeconds = 5f;

        private readonly WaitForSeconds _wait = new WaitForSeconds(LifetimeSeconds);
        private IMemoryPool _memoryPool;
        private Tween _tween;

        #region IPoolable

        void IPoolable<Vector3, Vector3, IMemoryPool>.OnSpawned(Vector3 p1, Vector3 p2, IMemoryPool p3)
        {
            _memoryPool = p3;

            transform.position = p1;

            var target = ExtendVector(p1, p2, 2);
            _tween = transform.DOMove(target, LifetimeSeconds);

            StartCoroutine(CountDownToDepawn());
        }

        void IPoolable<Vector3, Vector3, IMemoryPool>.OnDespawned()
        {
            _memoryPool = null;
            _tween?.Kill();
            _tween = null;
        }

        #endregion

        private IEnumerator CountDownToDepawn()
        {
            yield return _wait;
            _memoryPool.Despawn(this);
        }

        private Vector3 ExtendVector(Vector3 from, Vector3 to, float multiplier)
        {
            var delta = to - from;
            delta *= multiplier;
            return from + delta;
        }

        public class Factory : PlaceholderFactory<Vector3, Vector3, ZombieProjectile>
        {
        }
    }
}