using Zenject;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    public abstract class SelfDespawning : ISelfDespawning
    {
        protected IMemoryPool? PoolReference;

        public void DespawnSelf()
        {
            Asserting.NotNull(PoolReference);
            PoolReference?.Despawn(this);
        }
    }
}