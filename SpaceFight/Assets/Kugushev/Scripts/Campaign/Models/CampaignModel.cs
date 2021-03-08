using System;
using System.Collections.Generic;
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
        [SerializeReference] private List<AchievementInfo> achievements = new List<AchievementInfo>();

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

        public IReadOnlyList<AchievementInfo> Achievements => achievements;
        public void AddAchievement(AchievementInfo achievementInfo) => achievements.Add(achievementInfo);
    }
}