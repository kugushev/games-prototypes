using Kugushev.Scripts.Common.Utils.Pooling;
using Zenject;

namespace Kugushev.Scripts.Common.Modes
{
#nullable disable
    public class SignalToTransition<TParameters> : SelfDespawning<SignalToTransition<TParameters>.Pool>
    {
        public class Pool : MemoryPool<TParameters, SignalToTransition<TParameters>>
        {
            protected override void Reinitialize(TParameters p1, SignalToTransition<TParameters> item)
            {
                item._pool = this;

                item.Parameters = p1;
            }

            protected override void OnDespawned(SignalToTransition<TParameters> item)
            {
                item.Parameters = default;
            }
        }

        public TParameters Parameters { get; private set; }
    }
#nullable enable
}