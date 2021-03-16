using Kugushev.Scripts.Common.ValueObjects;
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
    [CreateAssetMenu(menuName = MenuName + nameof(Moska))]
    public class Moska : AbstractAchievement
    {
        [SerializeField] private int level;
        [SerializeField] private float powerCap;
        [SerializeField] private float multiplication;

        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Moska, level, AchievementType.Epic, nameof(Moska),
            $"Have armies only with no more {powerCap} power",
            $"Armies with less than {powerCap} power receive and deal less damage by {multiplication} on fights");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmySent)
                if (missionEvent.Owner == faction && missionEvent.Power > powerCap)
                    return false;

            return true;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties,
            ref PlanetarySystemPropertiesBuilder planetarySystemProperties)
        {
            ref var damage = ref fleetProperties.FightDamageMultiplication;
            if (damage.LowCap != null || damage.UnderCapEffect != null)
                Debug.LogError($"Damage effect is already specified {damage.LowCap} {damage.UnderCapEffect}");

            damage.LowCap = powerCap;
            damage.UnderCapEffect = new Percentage(multiplication);

            ref var protection = ref fleetProperties.FightProtectionMultiplication;
            if (protection.LowCap != null || protection.UnderCapEffect != null)
                Debug.LogError($"Protection effect is already specified {damage.LowCap} {damage.UnderCapEffect}");

            protection.LowCap = powerCap;
            protection.UnderCapEffect = new Percentage(multiplication);
        }
    }
}