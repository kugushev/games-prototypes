using Kugushev.Scripts.Campaign.Core.ValueObjects;
using Kugushev.Scripts.Campaign.Models;

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