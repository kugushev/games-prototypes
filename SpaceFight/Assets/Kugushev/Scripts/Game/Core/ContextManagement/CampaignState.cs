using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Constants;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    public class CampaignState: ParameterizedSceneLoadingState<CampaignParameters>
    {
        public CampaignState(ParametersPipeline<CampaignParameters> parametersPipeline) 
            : base(UnityConstants.CampaignManagementScene, false, parametersPipeline)
        {
        }
    }
}