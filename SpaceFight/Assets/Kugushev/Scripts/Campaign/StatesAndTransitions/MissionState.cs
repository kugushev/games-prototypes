using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class MissionState : BaseSceneLoadingState<CampaignModel>
    {
        private readonly MissionSceneParametersPipeline _missionSceneParametersPipeline;

        public MissionState(CampaignModel model, MissionSceneParametersPipeline missionSceneParametersPipeline)
            : base(model, UnityConstants.MissionManagementScene)
        {
            _missionSceneParametersPipeline = missionSceneParametersPipeline;
        }
        
        protected override void OnEnterBeforeLoadScene()
        {
            var campaignInfo = new MissionInfo(Model.NextMissionSeed);
            _missionSceneParametersPipeline.Set(campaignInfo);
        }
    }
}