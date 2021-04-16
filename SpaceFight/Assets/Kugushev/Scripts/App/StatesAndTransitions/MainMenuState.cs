using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.App.StatesAndTransitions
{
    internal class MainMenuState : BaseSceneLoadingState<AppModel>
    {
        public MainMenuState(AppModel model) : base(model, UnityConstants.MainMenuScene, true)
        {
        }

        protected override void AssertModel()
        {
        }
    }
}