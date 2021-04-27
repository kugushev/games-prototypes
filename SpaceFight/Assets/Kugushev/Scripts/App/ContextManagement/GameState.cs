using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.App.ContextManagement
{
    internal class GameState : ParameterizedSceneLoadingState<GameParameters>
    {
        public GameState(ParametersPipeline<GameParameters> parametersPipeline)
            : base(AppConstants.Scenes.GameManagementScene, true, parametersPipeline)
        {
        }
    }
}