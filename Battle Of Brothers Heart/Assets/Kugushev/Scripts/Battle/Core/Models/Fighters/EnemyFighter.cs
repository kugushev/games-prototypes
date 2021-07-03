using JetBrains.Annotations;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;

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