using Zenject;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class SelfDespawning<TPool> : ISelfDespawning
        where TPool : class, IMemoryPool
    {
        protected TPool? _pool;

        public void DespawnSelf()
        {
            Asserting.NotNull(_pool);
            _pool?.Despawn(this);
        }
    }
}