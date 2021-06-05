using System;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.Models;
using Zenject;

namespace Kugushev.Scripts.Core.Battle.ValueObjects.Orders
{
    public class OrderAttack : IOrder, IPoolable<EnemyUnit, IMemoryPool>, IDisposable
    {
        private IMemoryPool? _memoryPool;
        private EnemyUnit? _target;

        public EnemyUnit Target => _target ?? throw new Exception("Target has not specified");

        void IPoolable<EnemyUnit, IMemoryPool>.OnSpawned(EnemyUnit p1, IMemoryPool p2)
        {
            _target = p1;
            _memoryPool = p2;
        }

        void IPoolable<EnemyUnit, IMemoryPool>.OnDespawned()
        {
            _target = default;
            _memoryPool = default;
        }

        public void Dispose() => _memoryPool?.Despawn(this);

        public class Factory : PlaceholderFactory<EnemyUnit, OrderAttack>
        {
        }
    }
}