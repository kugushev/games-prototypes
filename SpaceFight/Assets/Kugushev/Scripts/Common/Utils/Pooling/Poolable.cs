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
            OnRestore(ObjectState);
        }

        protected virtual void OnRestore(TState state)
        {
        }

        public void ClearState()
        {
            OnClear(ObjectState);
            ObjectState = default;
        }
        
        protected virtual void OnClear(TState state)
        {
        }

        public void Dispose() => _myPool.GiveBackObject(this);
    }
}