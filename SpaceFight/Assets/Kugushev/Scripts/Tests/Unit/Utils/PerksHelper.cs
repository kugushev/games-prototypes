﻿using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Services;

namespace Kugushev.Scripts.Tests.Unit.Utils
{
    public static class PerksHelper
    {
        public static (PlanetarySystemPerks, FleetPerks) GetPlayerProperties(
            params (AchievementId, int? level, AchievementType)[] achievements)
        {
            var service = AssetBundleHelper.LoadAsset<PlayerPropertiesService>("Test Player Properties Service");

            var achievementsModel = new PlayerAchievements();
            foreach (var (achievementId, level, achievementType) in achievements)
            {
                achievementsModel.AddAchievement(
                    new AchievementInfo(achievementId, level, achievementType, "", "", ""));
            }

            return service.GetPlayerProperties(Faction.Green, new MissionParameters(default, achievementsModel));
        }
    }
}