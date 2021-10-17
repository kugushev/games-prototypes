using System;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.AI.Orders
{
    public class OrderMove : IOrder, IPoolable<Position, IMemoryPool>, IDisposable
    {
        private IMemoryPool? _memoryPool;

        public Position Target { get; private set; }


        void IPoolable<Position, IMemoryPool>.OnSpawned(Position p1, IMemoryPool p2)
        {
            Target = p1;
            _memoryPool = p2;
        }

        void IPoolable<Position, IMemoryPool>.OnDespawned()
        {
            Target = default;
            _memoryPool = default;
        }

        public void Dispose() => _memoryPool?.Despawn(this);

        public class Factory : PlaceholderFactory<Position, OrderMove>
        {
        }
    }
}