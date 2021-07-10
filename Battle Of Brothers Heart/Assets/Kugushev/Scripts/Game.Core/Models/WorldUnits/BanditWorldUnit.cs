using System.Collections.Generic;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Models.WorldUnits
{
    public class BanditWorldUnit : EnemyWorldUnit
    {
        public BanditWorldUnit(City homeCity, IReadOnlyList<Enemy> characters)
            : base(homeCity.Position, Direction2d.Down, characters)
        {
            HomeCity = homeCity;
        }

        public City HomeCity { get; }
    }
}