using System;

namespace Kugushev.Scripts.Game.Enums
{
    public enum Faction
    {
        Unspecified = 0,
        Green = 1,
        Neutral = 2,
        Red = 3
    }

    public static class FactionExtensions
    {
        public static Faction GetOpposite(this Faction faction) => faction switch
        {
            Faction.Green => Faction.Red,
            Faction.Red => Faction.Green,
            Faction.Neutral => Faction.Neutral,
            _ => throw new ArgumentOutOfRangeException(nameof(Faction), faction, "Unexpected army")
        };
    }
}