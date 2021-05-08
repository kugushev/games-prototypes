using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Campaign.Models
{
    public class Playground : PoolableOld<Playground.State>
    {
        public struct State
        {
            public int PlayerScore;
            public int AIScore;
            public int Seed;
            public int? PlayerHomeProductionMultiplier;
            public int? EnemyHomeProductionMultiplier;
            public int? PlayerExtraPlanets;
            public int? EnemyExtraPlanets;
            public int? PlayerStartPowerMultiplier;
            public int? EnemyStartPowerMultiplier;
        }

        public Playground(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public int PlayerScore
        {
            get => ObjectState.PlayerScore;
            set => ObjectState.PlayerScore = value;
        }

        public int AIScore
        {
            get => ObjectState.AIScore;
            set => ObjectState.AIScore = value;
        }

        public int Seed
        {
            get => ObjectState.Seed;
            set => ObjectState.Seed = value;
        }

        public int? PlayerHomeProductionMultiplier
        {
            get => ObjectState.PlayerHomeProductionMultiplier;
            set => ObjectState.PlayerHomeProductionMultiplier = value;
        }

        public int? EnemyHomeProductionMultiplier
        {
            get => ObjectState.EnemyHomeProductionMultiplier;
            set => ObjectState.EnemyHomeProductionMultiplier = value;
        }

        public int? PlayerExtraPlanets
        {
            get => ObjectState.PlayerExtraPlanets;
            set => ObjectState.PlayerExtraPlanets = value;
        }

        public int? EnemyExtraPlanets
        {
            get => ObjectState.EnemyExtraPlanets;
            set => ObjectState.EnemyExtraPlanets = value;
        }

        public int? PlayerStartPowerMultiplier
        {
            get => ObjectState.PlayerStartPowerMultiplier;
            set => ObjectState.PlayerStartPowerMultiplier = value;
        }

        public int? EnemyStartPowerMultiplier
        {
            get => ObjectState.EnemyStartPowerMultiplier;
            set => ObjectState.EnemyStartPowerMultiplier = value;
        }
    }
}