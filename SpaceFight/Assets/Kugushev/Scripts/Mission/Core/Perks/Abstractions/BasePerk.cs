using Kugushev.Scripts.Common;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;

namespace Kugushev.Scripts.Mission.Perks.Abstractions
{
    public abstract class BasePerk : Perk
    {
        protected const string MenuName = CommonConstants.MenuPrefix + "Achievements/";

        public abstract bool Check(EventsCollectingService missionEvents, Faction faction);

        public abstract void Apply(FleetEffects fleetEffects,
            PlanetarySystemEffects planetarySystemEffects);
    }
}