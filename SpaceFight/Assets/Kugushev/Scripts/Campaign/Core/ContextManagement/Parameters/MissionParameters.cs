using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters
{
    public readonly struct MissionParameters
    {
        public MissionParameters(MissionInfo missionInfo)
        {
            MissionInfo = missionInfo;
        }

        public MissionInfo MissionInfo { get; }
    }
}