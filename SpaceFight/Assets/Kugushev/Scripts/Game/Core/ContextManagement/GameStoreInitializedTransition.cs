using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    public class GameStoreInitializedTransition : ITransition
    {
        private readonly GameDataStore _gameDataStore;

        public GameStoreInitializedTransition(GameDataStore gameDataStore) => _gameDataStore = gameDataStore;

        public bool ToTransition => _gameDataStore.Initialized;
    }
}