using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class MissionState : BaseSceneLoadingState<CampaignModelOld>
    {
        private readonly MissionSceneParametersPipeline _missionSceneParametersPipeline;
        private readonly MissionSceneResultPipeline _missionSceneResultPipeline;

        public MissionState(CampaignModelOld modelOld, MissionSceneParametersPipeline missionSceneParametersPipeline,
            MissionSceneResultPipeline missionSceneResultPipeline)
            : base(modelOld, UnityConstants.MissionManagementScene, false)
        {
            _missionSceneParametersPipeline = missionSceneParametersPipeline;
            _missionSceneResultPipeline = missionSceneResultPipeline;
        }

        protected override void AssertModel()
        {
            if (ModelOld.NextMission == null)
                Debug.LogError("Next Mission has not specified");
        }

        protected override void OnEnterBeforeLoadScene()
        {
            ModelOld.LastMissionResult = null;

            var missionInfo = ModelOld.NextMission;
            var campaignInfo = new MissionParameters();
            _missionSceneParametersPipeline.Set(campaignInfo);
        }

        protected override void OnExitBeforeUnloadScene()
        {
            var result = _missionSceneResultPipeline.Get();

            ModelOld.LastMissionResult = result;

            if (result.ChosenPerk is { } reward)
                ModelOld.PlayerPerksOld.AddPerk(reward);

            if (result.PlayerWins)
                ModelOld.CampaignResult.AddReward(result.MissionInfo.Reward);

            ModelOld.NextMission = null;
        }
    }
}