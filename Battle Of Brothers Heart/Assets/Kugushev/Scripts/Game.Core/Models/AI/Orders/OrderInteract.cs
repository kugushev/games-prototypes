using System;
using Kugushev.Scripts.Game.Core.Interfaces;
using Kugushev.Scripts.Game.Core.Interfaces.AI;
using Zenject;

namespace Kugushev.Scripts.Game.Core.Models.AI.Orders
{
    public class OrderInteract<TSubject> : OrderInteract, IOrder, IPoolable<TSubject, IMemoryPool>, IDisposable
        where TSubject : class, IInteractable
    {
        private IMemoryPool? _memoryPool;
        private TSubject? _target;

        public override IInteractable Target => Victim;
        public TSubject Victim => _target ?? throw new Exception("Target has not specified");

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
        public abstract IInteractable Target { get; }
    }
}