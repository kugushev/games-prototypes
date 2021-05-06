using Kugushev.Scripts.App.Core.Constants;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.App.Core.ContextManagement
{
    internal class MainMenuState : UnparameterizedSceneLoadingState
    {
        public MainMenuState() : base(AppConstants.Scenes.MainMenuScene, true)
        {
        }
    }
}