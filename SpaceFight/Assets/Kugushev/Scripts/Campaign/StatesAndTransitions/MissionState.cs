using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            if (Model.NextMission == null)
                Debug.LogError("Next Mission has not specified");
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.LastMissionResult = null;

            var missionInfo = Model.NextMission ?? new MissionInfo();
            var campaignInfo = new MissionParameters(missionInfo, Model.PlayerAchievements);
            _missionSceneParametersPipeline.Set(campaignInfo);
        }

        protected override void OnExitBeforeUnloadScene()
        {
            var result = _missionSceneResultPipeline.Get();

            Model.LastMissionResult = result;

            if (result.Reward is { } reward)
                Model.PlayerAchievements.AddAchievement(reward);

            Model.NextMission = null;
        }
    }
}