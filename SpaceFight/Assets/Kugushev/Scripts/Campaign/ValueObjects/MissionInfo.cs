using System;
using Kugushev.Scripts.Campaign.Models;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    [Serializable]
    public readonly struct MissionInfo
    {
        public MissionInfo(MissionProperties missionProperties, PlayerAchievements playerAchievements)
        {
            PlayerAchievements = playerAchievements;
            MissionProperties = missionProperties;
        }

        public MissionProperties MissionProperties { get; }
        public PlayerAchievements PlayerAchievements { get; }
    }
}