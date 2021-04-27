using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Zenject;

namespace Kugushev.Scripts.AppPresentation.Signals
{
    internal class NewGameSignal : SelfDespawning<NewGameSignal.Pool>
    {
        internal class Pool : MemoryPool<GameModeParameters, NewGameSignal>
        {
            protected override void Reinitialize(GameModeParameters parameters, NewGameSignal item)
            {
                item._pool = this;
                
                item.Parameters = parameters;
            }
        }

        public GameModeParameters Parameters { get; private set; }
    }
}