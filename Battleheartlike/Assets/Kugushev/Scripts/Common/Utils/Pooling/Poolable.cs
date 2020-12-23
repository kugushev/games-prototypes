using System;
using UnityEngine;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class Poolable<TState> : IPoolable<TState>
        where TState : struct
    {
        protected Poolable(ObjectsPool objectsPool) => MyPool = objectsPool;

        public ObjectsPool MyPool { get; }
        public TState ObjectState { get; private set; }

        public void SetState(TState state) => ObjectState = state;
        public void ClearState() => ObjectState = default;

        public void Dispose() => MyPool.GiveBackObject(this);
    }
}