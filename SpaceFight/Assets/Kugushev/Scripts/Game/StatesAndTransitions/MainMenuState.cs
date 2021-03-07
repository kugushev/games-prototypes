using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    internal class MainMenuState : BaseSceneLoadingState<GameModel>
    {
        public MainMenuState(GameModel model) : base(model, UnityConstants.MainMenuScene, true)
        {
        }
    }
}