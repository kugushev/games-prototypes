using JetBrains.Annotations;
using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class EnemyFighter : BaseFighter
    {
        public bool IsBig { get; }

        public bool Burning { get; set; }

        public bool IsHeroHunter { get; }
        public bool IsConqueror { get; }

        public EnemyFighter(Position battlefieldPosition, Character character, Battlefield battlefield, bool isBig,
            bool isHeroHunter, bool isConqueror)
            : base(battlefieldPosition, character, battlefield)
        {
            IsBig = isBig;
            IsHeroHunter = isHeroHunter;
            IsConqueror = isConqueror;
        }
    }
}