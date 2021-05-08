#nullable disable
using System;
using Zenject;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class Poolable<TParam> : IPoolable<TParam, IMemoryPool>, IDisposable
    {
        protected TParam Parameter;
        private IMemoryPool _pool;

        void IPoolable<TParam, IMemoryPool>.OnSpawned(TParam p1, IMemoryPool p2)
        {
            Parameter = p1;
            _pool = p2;
        }

        void IPoolable<TParam, IMemoryPool>.OnDespawned()
        {
            if (Parameter is IDisposable disposable)
                disposable.Dispose();

            Parameter = default;
            _pool = null;
        }

        void IDisposable.Dispose()
        {
            _pool.Despawn(this);
        }
    }
}
#nullable enable