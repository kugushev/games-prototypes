using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class ToMissionTransition: ITransition
    {
        private readonly CampaignModel _campaignModel;

        public ToMissionTransition(CampaignModel campaignModel)
        {
            _campaignModel = campaignModel;
        }
        
        public bool ToTransition => _campaignModel.ReadyToNextMission;
    }
}