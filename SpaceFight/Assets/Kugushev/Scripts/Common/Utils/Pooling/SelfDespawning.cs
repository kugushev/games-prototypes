using Zenject;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class SelfDespawning<TPool> : ISelfDespawning
        where TPool : class, IMemoryPool
    {
        protected TPool? PoolReference;

        public void DespawnSelf()
        {
            Asserting.NotNull(PoolReference);
            PoolReference?.Despawn(this);
        }
    }
}