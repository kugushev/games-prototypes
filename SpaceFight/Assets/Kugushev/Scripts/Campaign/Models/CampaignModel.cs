using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Campaign.Models
{
    [Serializable]
    internal class CampaignModel : Poolable<CampaignModel.State>
    {
        public struct State
        {
            public CampaignInfo CampaignInfo;
            public readonly PlayerAchievements PlayerAchievements;
            public readonly MissionSelection MissionSelection;
            public readonly Playground Playground;
            public MissionInfo? NextMission;
            public MissionResult? LastMissionResult;

            public State(CampaignInfo campaignInfo, MissionSelection missionSelection, Playground playground)
            {
                CampaignInfo = campaignInfo;
                MissionSelection = missionSelection;
                Playground = playground;
                NextMission = null;
                LastMissionResult = null;
                PlayerAchievements = new PlayerAchievements();
            }
        }

        private readonly List<PoliticalAction> rewardedPoliticalActions =
            new List<PoliticalAction>(CampaignConstants.MissionsCount);

        public CampaignModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public CampaignInfo CampaignInfo => ObjectState.CampaignInfo;

        public MissionInfo? NextMission
        {
            get => ObjectState.NextMission;
            internal set => ObjectState.NextMission = value;
        }

        public MissionResult? LastMissionResult
        {
            get => ObjectState.LastMissionResult;
            internal set => ObjectState.LastMissionResult = value;
        }

        public PlayerAchievements PlayerAchievements => ObjectState.PlayerAchievements;
        public MissionSelection MissionSelection => ObjectState.MissionSelection;
        public Playground Playground => ObjectState.Playground;

        public void AddReward(PoliticalAction politicalAction) => rewardedPoliticalActions.Add(politicalAction);

        protected override void OnClear(State state)
        {
            state.MissionSelection.Dispose();
            state.Playground.Dispose();
            rewardedPoliticalActions.Clear();
        }

        protected override void OnRestore(State state) => rewardedPoliticalActions.Clear();
    }
}