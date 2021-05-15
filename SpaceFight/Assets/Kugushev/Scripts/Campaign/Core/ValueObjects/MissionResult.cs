using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionResult
    {
        public MissionResult(bool playerWins, MissionInfo missionInfo, PerkInfo? chosenPerk)
        {
            PlayerWins = playerWins;
            MissionInfo = missionInfo;
            ChosenPerk = chosenPerk;
        }

        public bool PlayerWins { get; }
        public MissionInfo MissionInfo { get; }
        public PerkInfo? ChosenPerk { get; }
    }
}