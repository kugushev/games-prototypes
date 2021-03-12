using JetBrains.Annotations;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Common
{
    [CreateAssetMenu(menuName = MenuName + nameof(Invader))]
    public class Invader : AbstractAchievement
    {
        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Invader, null, AchievementType.Common, nameof(Invader),
            "Invade to a planet", "Increased siege damage on 10%");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            [CanBeNull] MissionModel model)
        {
            foreach (var missionEvent in  missionEvents.PlanetCaptured)
                if (missionEvent.NewOwner == faction)
                    return true;
            return false;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties,
            ref PlanetarySystemPropertiesBuilder planetarySystemProperties)
        {
            fleetProperties.SiegeMultiplier += 0.1f;
        }
    }
}