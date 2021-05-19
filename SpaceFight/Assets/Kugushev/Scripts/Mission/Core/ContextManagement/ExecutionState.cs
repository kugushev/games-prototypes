using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Mission.Constants;

namespace Kugushev.Scripts.Mission.Core.ContextManagement
{
    public class ExecutionState : UnparameterizedSceneLoadingState
    {
        public ExecutionState()
            : base(UnityConstants.Scenes.MissionExecutionScene, true)
        {
        }
    }
}