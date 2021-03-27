using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Startup))]
    public class Startup : AbstractAchievement, IMultiplierPerk<Planet>
    {
        [SerializeField] private int level;
        [SerializeField] private float maxPower;
        [SerializeField] private float multiplier;
        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Startup, level, AchievementType.Epic, nameof(Startup),
            $"Recruit only on planets that have less than {maxPower} power",
            $"Increase production to {multiplier} if power is less than {maxPower}, decreased if more");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmySent)
                if (missionEvent.Owner == faction && missionEvent.RemainingPower + missionEvent.Power > maxPower)
                    return false;

            return true;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
            => planetarySystemPerks.production.AddPerk(this);

        public float? GetMultiplier(Planet criteria)
        {
            if (criteria.Power <= maxPower)
                return multiplier;
            return 1 / multiplier;
        }
    }
}