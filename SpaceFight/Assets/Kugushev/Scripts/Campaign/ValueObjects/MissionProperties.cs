using Kugushev.Scripts.Campaign.Models;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionProperties
    {
        public MissionProperties(NextMissionProperties builder)
        {
            Seed = builder.Seed;
            PlayerHomeProductionMultiplier = builder.PlayerHomeProductionMultiplier;
            EnemyHomeProductionMultiplier = builder.EnemyHomeProductionMultiplier;
            PlayerExtraPlanets = builder.PlayerExtraPlanets;
            EnemyExtraPlanets = builder.EnemyExtraPlanets;
            PlayerStartPower = builder.PlayerStartPower;
            EnemyStartPower = builder.EnemyStartPower;
        }

        public MissionProperties(int seed,
            int? playerHomeProductionMultiplier = null,
            int? enemyHomeProductionMultiplier = null,
            int? playerExtraPlanets = null,
            int? enemyExtraPlanets = null,
            int? playerStartPower = null,
            int? enemyStartPower = null)
        {
            Seed = seed;
            PlayerHomeProductionMultiplier = playerHomeProductionMultiplier;
            EnemyHomeProductionMultiplier = enemyHomeProductionMultiplier;
            PlayerExtraPlanets = playerExtraPlanets;
            EnemyExtraPlanets = enemyExtraPlanets;
            PlayerStartPower = playerStartPower;
            EnemyStartPower = enemyStartPower;
        }

        public int Seed { get; }
        public int? PlayerHomeProductionMultiplier { get; }
        public int? EnemyHomeProductionMultiplier { get; }
        public int? PlayerExtraPlanets { get; }
        public int? EnemyExtraPlanets { get; }
        public int? PlayerStartPower { get; }
        public int? EnemyStartPower { get; }
    }
}