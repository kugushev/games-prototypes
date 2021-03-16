using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Enums;
using NUnit.Framework;
using static Kugushev.Scripts.Tests.Unit.Utils.Factory;

namespace Kugushev.Scripts.Tests.Unit
{
    public class PlanetTests
    {
        [Test]
        public void ExecuteProductionCycle_NoAchievements_IncreasePowerToProduction()
        {
            // arrange
            var planet = CreatePlanet(4f, Faction.Green);

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(4f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl3_IncreasePowerX4OfProduction()
        {
            // arrange
            var planet = CreatePlanet(4f, Faction.Green, (AchievementId.Startup, 3, AchievementType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(16f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl2_IncreasePowerX2d5fProduction()
        {
            // arrange
            var planet = CreatePlanet(4f, Faction.Green, (AchievementId.Startup, 2, AchievementType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(10f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl1_IncreasePowerX1d5fProduction()
        {
            // arrange
            var planet = CreatePlanet(4f, Faction.Green, (AchievementId.Startup, 1, AchievementType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(6f, planet.Power);
        }
    }
}