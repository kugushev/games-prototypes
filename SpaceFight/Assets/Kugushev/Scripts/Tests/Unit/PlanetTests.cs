using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Services;
using Kugushev.Scripts.Tests.Unit.Utils;
using NUnit.Framework;

namespace Kugushev.Scripts.Tests.Unit
{
    public class PlanetTests
    {
        [Test]
        public void ExecuteProductionCycle_NoAchievements_IncreasePowerToProduction()
        {
            // arrange
            var planet = CreateAllyPlanet(4f);

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(4f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl3_IncreasePowerX4OfProduction()
        {
            // arrange
            var planet = CreateAllyPlanet(4f, (AchievementId.Startup, 3, AchievementType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(16f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl2_IncreasePowerX2d5fProduction()
        {
            // arrange
            var planet = CreateAllyPlanet(4f, (AchievementId.Startup, 2, AchievementType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(10f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl1_IncreasePowerX1d5fProduction()
        {
            // arrange
            var planet = CreateAllyPlanet(4f, (AchievementId.Startup, 1, AchievementType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(6f, planet.Power);
        }

        private Planet CreateAllyPlanet(float production,
            params (AchievementId, int level, AchievementType)[] achievements)
        {
            var service = AssetBundleHelper.LoadAsset<PlayerPropertiesService>("Test Player Properties Service");

            var achievementsModel = new PlayerAchievements();
            foreach (var (achievementId, level, achievementType) in achievements)
            {
                achievementsModel.AddAchievement(
                    new AchievementInfo(achievementId, level, achievementType, "", "", ""));
            }

            var (planetarySystemProperties, _) =
                service.GetPlayerProperties(Faction.Green, new MissionInfo(default, achievementsModel));

            var planet = new Planet(null);

            planet.SetState(new Planet.State(Faction.Green, default, production, default, default, default,
                planetarySystemProperties));

            return planet;
        }
    }
}