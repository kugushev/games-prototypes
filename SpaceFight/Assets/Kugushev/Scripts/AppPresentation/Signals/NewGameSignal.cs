using Kugushev.Scripts.App.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.AppPresentation.Signals
{
    public class NewGameSignal
    {
        public class Pool : MemoryPool<GameModeParameters, NewGameSignal>
        {
            protected override void Reinitialize(GameModeParameters parameters, NewGameSignal item)
            {
                item._pool = this;
                item.Parameters = parameters;
            }
        }

        private Pool _pool;

        public GameModeParameters Parameters { get; private set; }

        public void DespawnMyself() => _pool.Despawn(this);
    }
}