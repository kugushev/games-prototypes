using System.Collections.Generic;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Services
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(PlayerPropertiesService))]
    public class PlayerPropertiesService : ScriptableObject
    {
        [SerializeField] private AchievementsManager achievementsManager;

        private readonly List<AbstractAchievement> _achievementBuffer = new List<AbstractAchievement>(128);

        public (PlanetarySystemProperties, FleetProperties) GetPlayerProperties(Faction playerFaction, MissionParameters parameters)
        {
            _achievementBuffer.Clear();
            achievementsManager.FindMatched(_achievementBuffer, parameters.PlayerAchievements);

            var planetarySystemBuilder = new PlanetarySystemPropertiesBuilder();
            var fleetBuilder = new FleetPropertiesBuilder();
            foreach (var achievement in _achievementBuffer)
                achievement.Apply(ref fleetBuilder, ref planetarySystemBuilder);
           
            _achievementBuffer.Clear();

            var planetarySystemProperties = new PlanetarySystemProperties(playerFaction, planetarySystemBuilder);
            var fleetProperties = new FleetProperties(fleetBuilder);
            return (planetarySystemProperties, fleetProperties);
        }
    }
}