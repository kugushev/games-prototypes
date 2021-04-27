using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.App.ContextManagement
{
    public class MainMenuState : UnparameterizedSceneLoadingState
    {
        public MainMenuState() : base(AppConstants.Scenes.MainMenuScene, true)
        {
        }
    }
}