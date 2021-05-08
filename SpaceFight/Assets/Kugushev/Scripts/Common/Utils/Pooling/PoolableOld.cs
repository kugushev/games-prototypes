using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    [SuppressMessage("ReSharper", "Unity.RedundantSerializeFieldAttribute", Justification = "Used by inheritors")]
    public abstract class PoolableOld<TState> : IPoolable<TState>
        where TState : struct
    {
        private readonly ObjectsPool _myPool;
        [SerializeField] protected TState ObjectState;
        protected PoolableOld(ObjectsPool objectsPool) => _myPool = objectsPool;
        
        public bool Active { get; private set; }

        public void SetState(TState state)
        {
            ObjectState = state;
            OnRestore(ObjectState);
            Active = true;
        }

        protected virtual void OnRestore(TState state)
        {
        }

        public void ClearState()
        {
            OnClear(ObjectState);
            ObjectState = default;
            Active = false;
        }

        protected virtual void OnClear(TState state)
        {
        }

        public void Dispose()
        {
            if (Active)
                _myPool.GiveBackObject(this);
        }
    }
}