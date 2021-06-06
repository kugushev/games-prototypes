using System;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.Models;
using Zenject;

namespace Kugushev.Scripts.Core.Battle.ValueObjects.Orders
{
    public class OrderAttack : IOrder, IPoolable<BaseUnit, IMemoryPool>, IDisposable
    {
        private IMemoryPool? _memoryPool;
        private BaseUnit? _target;

        public BaseUnit Target => _target ?? throw new Exception("Target has not specified");

        void IPoolable<BaseUnit, IMemoryPool>.OnSpawned(BaseUnit p1, IMemoryPool p2)
        {
            _target = p1;
            _memoryPool = p2;
        }

        void IPoolable<BaseUnit, IMemoryPool>.OnDespawned()
        {
            _target = default;
            _memoryPool = default;
        }

        public void Dispose() => _memoryPool?.Despawn(this);

        public class Factory : PlaceholderFactory<BaseUnit, OrderAttack>
        {
        }
    }
}