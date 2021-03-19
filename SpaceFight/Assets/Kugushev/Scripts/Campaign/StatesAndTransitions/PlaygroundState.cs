using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.StatesAndTransitions;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class PlaygroundState : BaseSceneLoadingState<CampaignModel>
    {
        public PlaygroundState(CampaignModel model) : base(model, UnityConstants.PlaygroundScene, true)
        {
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.NextMissionProperties.Seed =
                Random.Range(CampaignConstants.MissionSeedMin, CampaignConstants.MissionSeedMax);
        }
    }
}