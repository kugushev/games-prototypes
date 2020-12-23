using System;
using UnityEngine;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class Poolable<TState> : IPoolable<TState>
        where TState : struct
    {
        private readonly ObjectsPool _myPool;
        protected TState ObjectState;
        protected Poolable(ObjectsPool objectsPool) => _myPool = objectsPool;

        public void SetState(TState state)
        {
            ObjectState = state;
            OnRestore();
        }

        protected virtual void OnRestore()
        {
        }

        public void ClearState() => ObjectState = default;

        public void Dispose() => _myPool.GiveBackObject(this);
    }
}