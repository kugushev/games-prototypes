using JetBrains.Annotations;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Models.WorldUnits
{
    public class PlayerWorldUnit: WorldUnit
    {
        public PlayerWorldUnit(Position position, Direction2d direction, Party party) 
            : base(position, direction, party)
        {
        }
    }
}