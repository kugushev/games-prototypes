using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.Common.Modes;

namespace Kugushev.Scripts.App.Modes
{
    public class MainMenuState : UnparameterizedSceneLoadingState
    {
        public MainMenuState() : base(UnityConstants.MainMenuScene, true)
        {
        }
    }
}