using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    public class GameDataInitializedTransition : ITransition
    {
        private readonly GameDataInitializer _gameDataInitializer;

        public GameDataInitializedTransition(GameDataInitializer gameDataInitializer) => _gameDataInitializer = gameDataInitializer;

        public bool ToTransition => _gameDataInitializer.Initialized;
    }
}