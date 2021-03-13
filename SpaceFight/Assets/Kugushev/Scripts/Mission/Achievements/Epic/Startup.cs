using System.Collections.Generic;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Startup))]
    public class Startup : AbstractAchievement
    {
        [SerializeField] private int level;
        [SerializeField] private float maxPower;
        [SerializeField] private float multiplier;
        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Startup, level, AchievementType.Epic, nameof(Startup),
            $"Keep no more {maxPower} power on planets",
            $"Increase production by {multiplier} if power is less than {maxPower}, decreased if more");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            MissionModel model)
        {
            if (model != null)
            {
                if (!AreAllPlanetsSatisfied(model.PlanetarySystem.Planets, faction))
                    return false;
            }
            else
                Debug.LogError("Model is null");

            foreach (var missionEvent in missionEvents.ArmySent)
                if (missionEvent.Owner == faction && missionEvent.RemainingPower > maxPower)
                    return false;

            return true;
        }

        private bool AreAllPlanetsSatisfied(IReadOnlyList<Planet> planets, Faction faction)
        {
            foreach (var planet in planets)
                if (planet.Faction == faction && planet.Power > maxPower)
                    return false;

            return true;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties,
            ref PlanetarySystemPropertiesBuilder planetarySystemProperties)
        {
            if (planetarySystemProperties.LowProductionCap != null ||
                planetarySystemProperties.LowProductionMultiplier != null ||
                planetarySystemProperties.AboveLowProductionMultiplier != null)
                Debug.LogError("Properties are already set");

            planetarySystemProperties.LowProductionCap = maxPower;
            planetarySystemProperties.LowProductionMultiplier = multiplier;
            planetarySystemProperties.AboveLowProductionMultiplier = 1 / multiplier;
        }
    }
}