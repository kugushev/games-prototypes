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

        void IDisposable.Dispose() => _pool.Despawn(this);
    }

    public abstract class Poolable<TParam1, TParam2, TParam3> : IPoolable<TParam1, TParam2, TParam3, IMemoryPool>,
        IDisposable
    {
        protected TParam1 Parameter1;
        protected TParam2 Parameter2;
        protected TParam3 Parameter3;
        private IMemoryPool _pool;

        void IPoolable<TParam1, TParam2, TParam3, IMemoryPool>.OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3,
            IMemoryPool p4)
        {
            Parameter1 = p1;
            Parameter2 = p2;
            Parameter3 = p3;
            _pool = p4;
        }

        void IPoolable<TParam1, TParam2, TParam3, IMemoryPool>.OnDespawned()
        {
            if (Parameter1 is IDisposable disposable1)
                disposable1.Dispose();

            if (Parameter2 is IDisposable disposable2)
                disposable2.Dispose();

            if (Parameter3 is IDisposable disposable3)
                disposable3.Dispose();

            _pool = null;
        }

        void IDisposable.Dispose() => _pool.Despawn(this);
    }
}
#nullable enable