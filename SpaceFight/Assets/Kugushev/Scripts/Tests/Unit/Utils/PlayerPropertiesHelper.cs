using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Services;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;

namespace Kugushev.Scripts.Tests.Unit.Utils
{
    public static class PlayerPropertiesHelper
    {
        public static (PlanetarySystemProperties, FleetProperties) GetPlayerProperties(
            params (AchievementId, int? level, AchievementType)[] achievements)
        {
            var service = AssetBundleHelper.LoadAsset<PlayerPropertiesService>("Test Player Properties Service");

            var achievementsModel = new PlayerAchievements();
            foreach (var (achievementId, level, achievementType) in achievements)
            {
                achievementsModel.AddAchievement(
                    new AchievementInfo(achievementId, level, achievementType, "", "", ""));
            }

            return service.GetPlayerProperties(Faction.Green, new MissionInfo(default, achievementsModel));
        }
    }
}