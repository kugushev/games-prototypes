using System;
using System.Collections.Generic;

namespace Kugushev.Scripts.App.Core.Enums
{
    public enum PerkId
    {
        Unspecified,

        // Common perks
        Invader,
        Brawler,

        // Epic perks
        Startup,
        Moska,
        Kamikaze,
        Elephant,
        LuckyIndustrialist,
        Negotiator,
        Briber,
        Transporter,
        Impregnable
    }

    public static class PerkIdHelper
    {
        public static readonly ISet<PerkId> AllPerks;

        static PerkIdHelper()
        {
            var perks = new HashSet<PerkId>();
            foreach (PerkId perk in Enum.GetValues(typeof(PerkId)))
                perks.Add(perk);
            AllPerks = perks;
        }
    }
}