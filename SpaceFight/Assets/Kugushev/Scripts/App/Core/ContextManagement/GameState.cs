using Kugushev.Scripts.App.Core.Constants;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.App.Core.ContextManagement
{
    internal class GameState : ParameterizedSceneLoadingState<GameParameters>
    {
        public GameState(ParametersPipeline<GameParameters> parametersPipeline)
            : base(AppConstants.Scenes.GameManagementScene, true, parametersPipeline)
        {
        }
    }
}