using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Constants;

namespace Kugushev.Scripts.Game.ContextManagement
{
    public class PoliticsState : UnparameterizedSceneLoadingState
    {
        protected PoliticsState()
            : base(UnityConstants.PoliticsMenuScene, true)
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            // todo: signal PoliticsStateLoaded, subscribe all Politician to the signal
        }
    }
}