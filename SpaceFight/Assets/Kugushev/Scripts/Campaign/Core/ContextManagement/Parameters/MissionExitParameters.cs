using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters
{
    public readonly struct MissionExitParameters
    {
        public MissionExitParameters(MissionInfo missionInfo, bool playerWins)
        {
            MissionInfo = missionInfo;
            PlayerWins = playerWins;
        }

        public MissionInfo MissionInfo { get; }
        public bool PlayerWins { get; }
    }
}