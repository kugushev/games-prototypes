using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.Core.ValueObjects
{
    public class MissionInfo
    {
        public MissionInfo(int seed,
            Difficulty difficulty,
            Intrigue reward,
            int? playerHomeProductionMultiplier = null,
            int? enemyHomeProductionMultiplier = null,
            int? playerExtraPlanets = null,
            int? enemyExtraPlanets = null,
            int? playerStartPowerMultiplier = null,
            int? enemyStartPowerMultiplier = null)
        {
            Seed = seed;
            Difficulty = difficulty;
            Reward = reward;
            PlayerHomeProductionMultiplier = playerHomeProductionMultiplier;
            EnemyHomeProductionMultiplier = enemyHomeProductionMultiplier;
            PlayerExtraPlanets = playerExtraPlanets;
            EnemyExtraPlanets = enemyExtraPlanets;
            PlayerStartPowerMultiplier = playerStartPowerMultiplier;
            EnemyStartPowerMultiplier = enemyStartPowerMultiplier;
        }

        public int Seed { get; }
        public Difficulty Difficulty { get; }
        public Intrigue Reward { get; }
        public int? PlayerHomeProductionMultiplier { get; }
        public int? EnemyHomeProductionMultiplier { get; }
        public int? PlayerExtraPlanets { get; }
        public int? EnemyExtraPlanets { get; }
        public int? PlayerStartPowerMultiplier { get; }
        public int? EnemyStartPowerMultiplier { get; }

        public override string ToString() => StringBag.FromInt(Seed);
    }
}