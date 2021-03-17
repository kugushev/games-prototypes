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
    [CreateAssetMenu(menuName = MenuName + nameof(Elephant))]
    public class Elephant: AbstractAchievement
    {
        [SerializeField] private int level;
        [SerializeField] private float powerCap;
        [SerializeField] private float multiplication;
        
        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Moska, level, AchievementType.Epic, nameof(Moska),
            $"Have armies only with more {powerCap} power",
            $"Armies with more than {powerCap} power deal more damage by {multiplication} on fights and on siege");
        
        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmySent)
                if (missionEvent.Owner == faction && missionEvent.Power < powerCap)
                    return false;

            return true;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties, ref PlanetarySystemPropertiesBuilder planetarySystemProperties)
        {
            ref var fightDamage = ref fleetProperties.FightDamageMultiplication;
            if (fightDamage.HighCap != null || fightDamage.OverCapEffect != null)
                Debug.LogError($"Damage effect is already specified {fightDamage.HighCap} {fightDamage.OverCapEffect}");

            fightDamage.HighCap = powerCap;
            fightDamage.OverCapEffect = new Percentage(multiplication);
            
            ref var siegeDamage = ref fleetProperties.SiegeDamageMultiplication;
            if (siegeDamage.HighCap != null || siegeDamage.OverCapEffect != null)
                Debug.LogError($"Damage effect is already specified {siegeDamage.HighCap} {siegeDamage.OverCapEffect}");

            siegeDamage.HighCap = powerCap;
            siegeDamage.OverCapEffect = new Percentage(multiplication);
        }
    }
}