using System;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    [Serializable]
    internal class CampaignModel
    {
        [SerializeField] private CampaignInfo campaignInfo;
        [SerializeField] private int playerScore;
        [SerializeField] private int aiScore;
        [SerializeField] private int nextMissionSeed;

        public CampaignModel(CampaignInfo campaignInfo) => this.campaignInfo = campaignInfo;

        public CampaignInfo CampaignInfo => campaignInfo;

        public int PlayerScore
        {
            get => playerScore;
            set => playerScore = value;
        }

        public int AIScore
        {
            get => aiScore;
            set => aiScore = value;
        }

        public int NextMissionSeed
        {
            get => nextMissionSeed;
            set => nextMissionSeed = value;
        }
    }
}