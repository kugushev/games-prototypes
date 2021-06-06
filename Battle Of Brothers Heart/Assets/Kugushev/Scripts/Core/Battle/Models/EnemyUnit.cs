using Kugushev.Scripts.Core.Battle.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.Models
{
    public class EnemyUnit : BaseUnit
    {
        public EnemyUnit()
        {
            _position.Value = new Position(new Vector2(8, 0));
        }
    }
}