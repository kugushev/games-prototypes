using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters
{
    public readonly struct MissionExitParameters
    {
        public MissionExitParameters(MissionInfo missionInfo, bool playerWins, PerkInfo? chosenPerk)
        {
            MissionInfo = missionInfo;
            PlayerWins = playerWins;
            ChosenPerk = chosenPerk;
        }

        public MissionInfo MissionInfo { get; }
        public bool PlayerWins { get; }
        public PerkInfo? ChosenPerk { get; }
    }
}