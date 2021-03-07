using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.StatesAndTransitions;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class CampaignProgressState : BaseSceneLoadingState<CampaignModel>
    {
        public CampaignProgressState(CampaignModel model) : base(model, UnityConstants.CampaignProgressScene, true)
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.NextMissionSeed = Random.Range(CampaignConstants.MissionSeedMin, CampaignConstants.MissionSeedMax);
        }
    }
}