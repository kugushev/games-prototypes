using System;
using Kugushev.Scripts.Campaign.Models;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    [Serializable]
    public readonly struct MissionParameters
    {
        public MissionParameters(MissionInfo missionInfo, PlayerPerks playerPerks)
        {
            PlayerPerks = playerPerks;
            MissionInfo = missionInfo;
        }

        public MissionInfo MissionInfo { get; }
        public PlayerPerks PlayerPerks { get; }
    }
}