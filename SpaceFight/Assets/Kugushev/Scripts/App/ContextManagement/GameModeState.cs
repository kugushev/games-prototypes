using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.App.ContextManagement
{
    public class GameModeState : ParameterizedSceneLoadingState<GameContextParameters>
    {
        public GameModeState(ParametersPipeline<GameContextParameters> parametersPipeline)
            : base(AppConstants.Scenes.GameManagementScene, true, parametersPipeline)
        {
        }
    }
}