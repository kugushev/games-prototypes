namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class Poolable<TState> : IPoolable<TState>
        where TState : struct
    {
        private readonly ObjectsPool _myPool;
        private bool active;
        protected TState ObjectState;
        protected Poolable(ObjectsPool objectsPool) => _myPool = objectsPool;

        public void SetState(TState state)
        {
            ObjectState = state;
            OnRestore(ObjectState);
            active = true;
        }

        protected virtual void OnRestore(TState state)
        {
        }

        public void ClearState()
        {
            OnClear(ObjectState);
            ObjectState = default;
            active = false;
        }

        protected virtual void OnClear(TState state)
        {
        }

        public void Dispose()
        {
            if (active)
                _myPool.GiveBackObject(this);
        }
    }
}