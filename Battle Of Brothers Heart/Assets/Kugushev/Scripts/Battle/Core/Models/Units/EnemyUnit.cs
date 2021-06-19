using JetBrains.Annotations;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Units
{
    public class EnemyUnit : BaseUnit
    {
        public EnemyUnit(Position position, [NotNull] Battlefield battlefield) : base(position, battlefield)
        {
        }
    }
}