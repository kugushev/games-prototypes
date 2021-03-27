using Kugushev.Scripts.Campaign.Enums;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionInfo
    {
        public MissionInfo(int seed,
            Difficulty difficulty,
            int? playerHomeProductionMultiplier = null,
            int? enemyHomeProductionMultiplier = null,
            int? playerExtraPlanets = null,
            int? enemyExtraPlanets = null,
            int? playerStartPowerMultiplier = null,
            int? enemyStartPowerMultiplier = null)
        {
            Seed = seed;
            Difficulty = difficulty;
            PlayerHomeProductionMultiplier = playerHomeProductionMultiplier;
            EnemyHomeProductionMultiplier = enemyHomeProductionMultiplier;
            PlayerExtraPlanets = playerExtraPlanets;
            EnemyExtraPlanets = enemyExtraPlanets;
            PlayerStartPowerMultiplier = playerStartPowerMultiplier;
            EnemyStartPowerMultiplier = enemyStartPowerMultiplier;
        }

        public int Seed { get; }
        public Difficulty Difficulty { get; }
        public int? PlayerHomeProductionMultiplier { get; }
        public int? EnemyHomeProductionMultiplier { get; }
        public int? PlayerExtraPlanets { get; }
        public int? EnemyExtraPlanets { get; }
        public int? PlayerStartPowerMultiplier { get; }
        public int? EnemyStartPowerMultiplier { get; }

        #region Equality

        public bool Equals(MissionInfo other)
        {
            return Seed == other.Seed && Difficulty == other.Difficulty &&
                   PlayerHomeProductionMultiplier == other.PlayerHomeProductionMultiplier &&
                   EnemyHomeProductionMultiplier == other.EnemyHomeProductionMultiplier &&
                   PlayerExtraPlanets == other.PlayerExtraPlanets && EnemyExtraPlanets == other.EnemyExtraPlanets &&
                   PlayerStartPowerMultiplier == other.PlayerStartPowerMultiplier &&
                   EnemyStartPowerMultiplier == other.EnemyStartPowerMultiplier;
        }

        public override bool Equals(object obj) => obj is MissionInfo other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Seed;
                hashCode = (hashCode * 397) ^ (int) Difficulty;
                hashCode = (hashCode * 397) ^ PlayerHomeProductionMultiplier.GetHashCode();
                hashCode = (hashCode * 397) ^ EnemyHomeProductionMultiplier.GetHashCode();
                hashCode = (hashCode * 397) ^ PlayerExtraPlanets.GetHashCode();
                hashCode = (hashCode * 397) ^ EnemyExtraPlanets.GetHashCode();
                hashCode = (hashCode * 397) ^ PlayerStartPowerMultiplier.GetHashCode();
                hashCode = (hashCode * 397) ^ EnemyStartPowerMultiplier.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(MissionInfo left, MissionInfo right) => left.Equals(right);

        public static bool operator !=(MissionInfo left, MissionInfo right) => !left.Equals(right);

        #endregion
    }
}