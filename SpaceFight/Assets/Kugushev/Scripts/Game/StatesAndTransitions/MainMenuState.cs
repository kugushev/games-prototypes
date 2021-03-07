using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    internal class MainMenuState : BaseSceneLoadingState<GameModel>
    {
        public MainMenuState(GameModel model) : base(model, UnityConstants.MainMenuScene, true)
        {
        }

        protected override void AssertModel()
        {
            if (Model.MainMenu == null)
                Debug.LogError("Main menu model is null");
        }
    }
}