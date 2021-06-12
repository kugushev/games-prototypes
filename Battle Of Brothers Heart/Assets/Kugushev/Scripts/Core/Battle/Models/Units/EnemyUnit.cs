using JetBrains.Annotations;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.Models.Units
{
    public class EnemyUnit : BaseUnit
    {
        public EnemyUnit(Position position, [NotNull] Battlefield battlefield) : base(position, battlefield)
        {
        }
    }
}