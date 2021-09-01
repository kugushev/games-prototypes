using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Common
{
    public class Projectile : MonoBehaviour, IPoolable<Vector3, Vector3, float, IMemoryPool>
    {
        private const float MaxLifetimeSeconds = 5f;
        private IMemoryPool _memoryPool;

        private Vector3 _start;
        private Vector3 _finish;
        private float _lifetime;


        void IPoolable<Vector3, Vector3, float, IMemoryPool>.OnSpawned(Vector3 start, Vector3 direction, float speed,
            IMemoryPool pool)
        {
            _memoryPool = pool;

            _start = transform.position = start;
            _finish = GetFinish(start, direction, speed);
            _lifetime = 0f;
        }

        void IPoolable<Vector3, Vector3, float, IMemoryPool>.OnDespawned()
        {
            _memoryPool = null;
            _start = default;
            _finish = default;
            _lifetime = default;
        }

        private void Update()
        {
            _lifetime += Time.deltaTime;
            transform.position = Vector3.Lerp(_start, _finish, _lifetime / MaxLifetimeSeconds);

            if (_lifetime > MaxLifetimeSeconds)
                _memoryPool.Despawn(this);
        }
        
        private Vector3 GetFinish(Vector3 start, Vector3 direction, float speed)
        {
            var distance = speed * MaxLifetimeSeconds;
            var trail = direction.normalized * distance;
            return start + trail;
        }

        public class Factory : PlaceholderFactory<Vector3, Vector3, float, Projectile>
        {
        }
    }
}