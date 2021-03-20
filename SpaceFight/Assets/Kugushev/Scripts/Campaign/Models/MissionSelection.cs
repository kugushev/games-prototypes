using System.Collections.Generic;
using Kugushev.Scripts.Campaign.ValueObjects;

namespace Kugushev.Scripts.Campaign.Models
{
    public class MissionSelection
    {
        public const int MissionsCount = NormalMissionsCount + HardMissionsCount + InsaneMissionsCount;
        public const int NormalMissionsCount = 7;
        public const int HardMissionsCount = 5;
        public const int InsaneMissionsCount = 3;

        public IReadOnlyList<MissionInfo> Missions { get; internal set; }

        public MissionInfo? SelectedMission { get; set; }
    }
}