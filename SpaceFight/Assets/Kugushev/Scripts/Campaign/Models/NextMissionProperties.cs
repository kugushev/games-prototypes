using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Campaign.Models
{
    public class NextMissionProperties
    {
        public int Seed { get; set; }
        public int? PlayerHomeProductionMultiplier { get; set; }
        public int? EnemyHomeProductionMultiplier { get; set; }
        public int? PlayerExtraPlanets { get; set; }
        public int? EnemyExtraPlanets { get; set; }
        public int? PlayerStartPower { get; set; }
        public int? EnemyStartPower { get; set; }
    }
}