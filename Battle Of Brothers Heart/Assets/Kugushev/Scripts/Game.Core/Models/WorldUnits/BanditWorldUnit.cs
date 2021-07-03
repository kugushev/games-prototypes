using JetBrains.Annotations;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Models.WorldUnits
{
    public class BanditWorldUnit : WorldUnit
    {
        public BanditWorldUnit(City homeCity, Party party)
            : base(homeCity.Position, Direction2d.Down, party)
        {
            HomeCity = homeCity;
        }

        public City HomeCity { get; }
    }
}