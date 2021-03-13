using JetBrains.Annotations;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Abstractions
{
    public abstract class AbstractAchievement : ScriptableObject
    {
        protected const string MenuName = CommonConstants.MenuPrefix + "Achievements/";

        // todo: use for localization https://docs.unity3d.com/Packages/com.unity.localization@0.10/manual/QuickStartGuide.html

        public abstract AchievementInfo Info { get; }

        public abstract bool Check(MissionEventsCollector missionEvents, Faction faction,
            [CanBeNull] MissionModel model);

        public abstract void Apply(ref FleetPropertiesBuilder fleetProperties,
            ref PlanetarySystemPropertiesBuilder planetarySystemProperties);
    }
}