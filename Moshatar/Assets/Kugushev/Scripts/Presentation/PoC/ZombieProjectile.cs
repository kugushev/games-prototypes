using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class ZombieProjectile : MonoBehaviour, IPoolable<Vector3, Vector3, IMemoryPool>
    {
        private const float ToHeroDuration = 5f;
        private const float LifetimeSeconds = 10f;

        private readonly WaitForSeconds _wait = new WaitForSeconds(LifetimeSeconds);
        private IMemoryPool _memoryPool;
        private Tween _tween;

        void IPoolable<Vector3, Vector3, IMemoryPool>.OnSpawned(Vector3 p1, Vector3 p2, IMemoryPool p3)
        {
            _memoryPool = p3;

            transform.position = p1;
            _tween = transform.DOMove(p2, ToHeroDuration);
            
            StartCoroutine(CountDownToDepawn());
        }

        void IPoolable<Vector3, Vector3, IMemoryPool>.OnDespawned()
        {
            _memoryPool = null;
            _tween?.Kill();
            _tween = null;
        }

        private IEnumerator CountDownToDepawn()
        {
            yield return _wait;
            _memoryPool.Despawn(this);
        }
        
        public class Factory: PlaceholderFactory<Vector3, Vector3, ZombieProjectile>
        {
            
        }
    }
}