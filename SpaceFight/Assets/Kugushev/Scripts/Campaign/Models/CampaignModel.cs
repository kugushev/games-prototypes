using System;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Campaign.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    // todo: add pooling
    [Serializable]
    internal class CampaignModel
    {
        [SerializeField] private CampaignInfo campaignInfo;


        [SerializeReference] private PlayerAchievements playerAchievements = new PlayerAchievements();

        public CampaignModel(CampaignInfo campaignInfo) => this.campaignInfo = campaignInfo;

        public CampaignInfo CampaignInfo => campaignInfo;
        
        public MissionInfo? NextMission { get; internal set; }
        public MissionResult? LastMissionResult { get; internal set; }

        public PlayerAchievements PlayerAchievements => playerAchievements;
        public MissionSelection MissionSelection { get; } = new MissionSelection();
        public Playground Playground { get; } = new Playground();
    }
}