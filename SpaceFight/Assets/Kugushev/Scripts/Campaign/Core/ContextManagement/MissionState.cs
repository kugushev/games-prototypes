using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.Campaign.Core.ContextManagement
{
    public class MissionState : ParameterizedSceneLoadingState<MissionParameters>
    {
        public MissionState(ParametersPipeline<MissionParameters> parametersPipeline)
            : base(UnityConstants.MissionManagementScene, false, parametersPipeline)
        {
        }
    }
}