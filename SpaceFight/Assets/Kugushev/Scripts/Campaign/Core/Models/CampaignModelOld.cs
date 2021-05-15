using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Core.ValueObjects;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.Models
{
    [Serializable]
    public class CampaignModelOld : PoolableOld<CampaignModelOld.State>
    {
        public struct State
        {
            public CampaignParameters CampaignParameters;
            public readonly PlayerPerksOld PlayerPerksOld;
            public readonly MissionSelection MissionSelection;
            public readonly Playground Playground;
            public MissionInfo? NextMission;
            public MissionResult? LastMissionResult;

            // don't need to dispose: the lifetime of the object passes to Game
            public CampaignResult CampaignResult;

            public State(CampaignParameters campaignParameters, MissionSelection missionSelection, Playground playground,
                PlayerPerksOld playerPerksOld, CampaignResult campaignResult)
            {
                CampaignParameters = campaignParameters;
                MissionSelection = missionSelection;
                Playground = playground;
                NextMission = null;
                LastMissionResult = null;
                PlayerPerksOld = playerPerksOld;
                CampaignResult = campaignResult;
            }
        }

        private readonly List<Intrigue> rewardedPoliticalActions =
            new List<Intrigue>(CampaignConstants.MissionsCount);

        public CampaignModelOld(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public CampaignParameters CampaignParameters => ObjectState.CampaignParameters;

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

        public PlayerPerksOld PlayerPerksOld => ObjectState.PlayerPerksOld;
        public MissionSelection MissionSelection => ObjectState.MissionSelection;
        public Playground Playground => ObjectState.Playground;
        public CampaignResult CampaignResult => ObjectState.CampaignResult;

        protected override void OnClear(State state)
        {
            state.MissionSelection.Dispose();
            state.Playground.Dispose();
            state.PlayerPerksOld.Dispose();
            rewardedPoliticalActions.Clear();
        }

        protected override void OnRestore(State state) => rewardedPoliticalActions.Clear();
    }
}