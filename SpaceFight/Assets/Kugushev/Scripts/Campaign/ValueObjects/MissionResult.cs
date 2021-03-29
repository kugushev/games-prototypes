using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionResult
    {
        public MissionResult(bool playerWins, MissionInfo missionInfo, PerkInfo? reward)
        {
            PlayerWins = playerWins;
            MissionInfo = missionInfo;
            Reward = reward;
        }

        public bool PlayerWins { get; }
        public MissionInfo MissionInfo { get; }
        public PerkInfo? Reward { get; }
    }
}