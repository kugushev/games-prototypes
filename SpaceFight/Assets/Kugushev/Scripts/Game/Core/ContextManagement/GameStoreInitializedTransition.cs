using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    public class GameStoreInitializedTransition : ITransition
    {
        private readonly GameDateStore _gameDateStore;

        public GameStoreInitializedTransition(GameDateStore gameDateStore) => _gameDateStore = gameDateStore;

        public bool ToTransition => _gameDateStore.Initialized;
    }
}