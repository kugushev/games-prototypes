using System;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Campaign.Models
{
    [Serializable]
    internal class CampaignModel
    {
        public CampaignModel(CampaignInfo campaignInfo) => CampaignInfo = campaignInfo;

        public CampaignInfo CampaignInfo { get; }
    }
}