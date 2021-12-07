using JetBrains.Annotations;
using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class EnemyFighter : BaseFighter
    {
        public bool IsBig { get; }
        
        public EnemyFighter(Position battlefieldPosition, Character character, Battlefield battlefield, bool isBig)
            : base(battlefieldPosition, character, battlefield)
        {
            IsBig = isBig;
        }
    }
}