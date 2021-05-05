using System;
using Zenject;


namespace Kugushev.Scripts.Common.ContextManagement
{
#nullable disable
    public class SignalToTransition<TParameters> : IPoolable<TParameters, IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool;

        public TParameters Parameters { get; private set; }

        void IPoolable<TParameters, IMemoryPool>.OnDespawned() => _pool = null;

        void IPoolable<TParameters, IMemoryPool>.OnSpawned(TParameters p1, IMemoryPool p2)
        {
            Parameters = p1;
            _pool = p2;
        }

        void IDisposable.Dispose()
        { 
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<TParameters, SignalToTransition<TParameters>>
        {
        }
    }
#nullable enable
}