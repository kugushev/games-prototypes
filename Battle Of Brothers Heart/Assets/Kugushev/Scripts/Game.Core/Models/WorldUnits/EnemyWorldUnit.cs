using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Models.WorldUnits
{
    public abstract class EnemyWorldUnit : WorldUnit
    {
        protected EnemyWorldUnit(Position position, Direction2d direction, IReadOnlyList<Enemy> characters)
            : base(position, direction)
        {
            Characters = characters;
        }

        public IReadOnlyList<Enemy> Characters { get; }
    }
}