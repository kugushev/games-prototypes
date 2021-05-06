using JetBrains.Annotations;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;

namespace Kugushev.Scripts.Mission.Perks.Abstractions
{
    public abstract class BasePerk : Perk
    {
        protected const string MenuName = CommonConstants.MenuPrefix + "Achievements/";

        // todo: use for localization https://docs.unity3d.com/Packages/com.unity.localization@0.10/manual/QuickStartGuide.html

       

        public abstract bool Check(MissionEventsCollector missionEvents, Faction faction,
            [CanBeNull] MissionModel model);

        public abstract void Apply(ref FleetPerks.State fleetPerks,
            ref PlanetarySystemPerks.State planetarySystemPerks);
    }
}