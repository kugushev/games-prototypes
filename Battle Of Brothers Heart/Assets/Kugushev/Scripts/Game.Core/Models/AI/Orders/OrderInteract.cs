using System;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Game.Core.Interfaces;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Orders
{
    public class OrderInteract<TSubject> : OrderInteract, IOrder, IPoolable<TSubject, IMemoryPool>, IDisposable
        where TSubject : class, IInteractable
    {
        private IMemoryPool? _memoryPool;
        private TSubject? _target;

        public override IInteractable Interactable => Target;
        public TSubject Target => _target ?? throw new Exception("Target has not specified");

        void IPoolable<TSubject, IMemoryPool>.OnSpawned(TSubject p1, IMemoryPool p2)
        {
            _target = p1;
            _memoryPool = p2;
        }

        void IPoolable<TSubject, IMemoryPool>.OnDespawned()
        {
            _target = default;
            _memoryPool = default;
        }

        public void Dispose() => _memoryPool?.Despawn(this);
    }

    public abstract class OrderInteract
    {
        public abstract IInteractable Interactable { get; }
    }
}