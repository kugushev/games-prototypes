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
        private readonly MissionSceneResultPipeline _missionSceneResultPipeline;

        public MissionState(CampaignModel model, MissionSceneParametersPipeline missionSceneParametersPipeline,
            MissionSceneResultPipeline missionSceneResultPipeline)
            : base(model, UnityConstants.MissionManagementScene, false)
        {
            _missionSceneParametersPipeline = missionSceneParametersPipeline;
            _missionSceneResultPipeline = missionSceneResultPipeline;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var campaignInfo = new MissionInfo(Model.NextMissionSeed, Model.PlayerAchievements);
            _missionSceneParametersPipeline.Set(campaignInfo);
        }

        protected override void OnExitBeforeUnloadScene()
        {
            var result = _missionSceneResultPipeline.Get();
            if (result.PlayerWin)
                Model.PlayerScore++;
            else
                Model.AIScore++;

            if (result.Reward != null)
                Model.AddAchievement(result.Reward.Value);
        }
    }
}