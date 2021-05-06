using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.App.ContextManagement
{
    internal class MainMenuState : UnparameterizedSceneLoadingState
    {
        public MainMenuState() : base(AppConstants.Scenes.MainMenuScene, true)
        {
        }
    }
}