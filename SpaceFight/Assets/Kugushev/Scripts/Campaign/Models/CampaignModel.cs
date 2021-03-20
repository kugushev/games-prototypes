using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    // todo: add pooling
    [Serializable]
    internal class CampaignModel
    {
        [SerializeField] private CampaignInfo campaignInfo;
        [SerializeField] private int playerScore;
        [SerializeField] private int aiScore;
        [SerializeField] private int nextMissionSeed;


        [SerializeReference] private PlayerAchievements playerAchievements = new PlayerAchievements();

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

        public NextMissionProperties NextMissionProperties { get; } = new NextMissionProperties();

        public PlayerAchievements PlayerAchievements => playerAchievements;

        [CanBeNull] public MissionSelection MissionSelection { get; } = new MissionSelection();
    }
}