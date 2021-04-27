using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Modes;

namespace Kugushev.Scripts.App.Modes
{
    public class GameModeState : ParameterizedSceneLoadingState<GameModeParameters>
    {
        public GameModeState(ParametersPipeline<GameModeParameters> parametersPipeline)
            : base(UnityConstants.GameManagementScene, true, parametersPipeline)
        {
        }
    }
}