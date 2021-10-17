using JetBrains.Annotations;
using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class EnemyFighter : BaseFighter
    {
        public EnemyFighter(Position battlefieldPosition, Character character, Battlefield battlefield)
            : base(battlefieldPosition, character, battlefield)
        {
        }
    }
}