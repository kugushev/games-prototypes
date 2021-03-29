using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Game.ValueObjects;

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