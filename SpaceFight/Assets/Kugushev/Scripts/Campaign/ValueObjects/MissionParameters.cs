using System;
using Kugushev.Scripts.Campaign.Models;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    [Serializable]
    public readonly struct MissionParameters
    {
        public MissionParameters(MissionInfo missionInfo, PlayerAchievements playerAchievements)
        {
            PlayerAchievements = playerAchievements;
            MissionInfo = missionInfo;
        }

        public MissionInfo MissionInfo { get; }
        public PlayerAchievements PlayerAchievements { get; }
    }
}