using System;
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
    [CreateAssetMenu(menuName = MenuName + nameof(Kamikaze))]
    public class Kamikaze : AbstractAchievement
    {
        [SerializeField] private int level;
        [SerializeField] private int losses;
        [SerializeField] private float deathStrike;

        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Kamikaze, level, AchievementType.Epic, nameof(Kamikaze),
            $"Lose {losses} armies in fights",
            $"Armies makes {deathStrike} to one target before death");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            var deaths = 0;
            foreach (var missionEvent in missionEvents.ArmyDestroyedInFight)
            {
                if (missionEvent.Victim == faction)
                    deaths++;
            }
            return deaths >= losses;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties,
            ref PlanetarySystemPropertiesBuilder planetarySystemProperties)
        {
            if (fleetProperties.DeathStrike != null)
                Debug.LogError($"Death strike is already specified {fleetProperties.DeathStrike}");
            fleetProperties.DeathStrike = deathStrike;
        }
    }
}