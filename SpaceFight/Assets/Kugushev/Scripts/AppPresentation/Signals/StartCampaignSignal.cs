using Kugushev.Scripts.Common.Utils.Pooling;
using Zenject;

namespace Kugushev.Scripts.AppPresentation.Signals
{
    internal class StartCampaignSignal : SelfDespawning<StartCampaignSignal.Pool>
    {
        internal class Pool : MemoryPool<int, StartCampaignSignal>
        {
            protected override void Reinitialize(int p1, StartCampaignSignal item)
            {
                item._pool = this;
            }
        }
    }
}