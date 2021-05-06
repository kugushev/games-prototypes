using System.Collections.Generic;
using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using NUnit.Framework;
using static Kugushev.Scripts.Tests.Unit.Utils.TestData;
using Assert = NUnit.Framework.Assert;

namespace Kugushev.Scripts.Tests.Unit
{
    public class PlanetTests
    {
        #region ExecuteProductionCycle

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
            var planet = CreatePlanet(4f, Faction.Green, (PerkId.Startup, 3, PerkType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(16f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl2_IncreasePowerX2d5fProduction()
        {
            // arrange
            var planet = CreatePlanet(4f, Faction.Green, (PerkId.Startup, 2, PerkType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(10f, planet.Power);
        }

        [Test]
        public void ExecuteProductionCycle_AchievedStartupLvl1_IncreasePowerX1d5fProduction()
        {
            // arrange
            var planet = CreatePlanet(4f, Faction.Green, (PerkId.Startup, 1, PerkType.Epic));

            // act
            planet.ExecuteProductionCycle();

            // assert
            Assert.AreEqual(6f, planet.Power);
        }

        private static IEnumerable<(int percent, float planetPower, bool expectedResult, int expectedArmyPower,
            float expectedPlanetRemainingPower)> NoAchievementsCases => new[]
        {
            (100, 0f, false, 0, 0f),
            (0, 10f, false, 0, 10f),
            (10, 10f, true, 1, 9f),
            (25, 10f, true, 2, 8f),
            (50, 10f, true, 5, 5f),
            (75, 10f, true, 7, 3f),
            (100, 10f, true, 10, 0f),
            (10, 1f, true, 1, 0f),
            (100, GameplayConstants.SoftCapArmyPower + 5f, true, GameplayConstants.SoftCapArmyPower, 5f),
        };

        #endregion

        #region TryRecruit

        [Test]
        public void TryRecruit_NoAchievements_DecrementPowerBasedOnPortToRecruit(
            [ValueSource(nameof(NoAchievementsCases))]
            (int percent, float planetPower, bool expectedResult, int expectedArmyPower, float
                expectedPlanetRemainingPower) testCase)
        {
            // arrange
            var planet = CreatePlanet(testCase.planetPower, Faction.Green);

            // act
            planet.ExecuteProductionCycle();
            var recruited = planet.TryRecruit(new Percentage(testCase.percent), out var armyPower);

            // assert
            Assert.AreEqual(testCase.expectedResult, recruited);
            Assert.AreEqual(testCase.expectedArmyPower, armyPower);
            Assert.AreEqual(testCase.expectedPlanetRemainingPower, planet.Power);
        }

        private static IEnumerable<(int percent, float planetPower, int level, float substitudeRandomRange,
            bool expectedResult, int expectedArmyPower, float expectedPlanetRemainingPower)> LuckyIndustrialistCases =>
            new[]
            {
                (100, 10f, 1, 0.26f, true, 10, 0f),
                (100, 10f, 1, 0.25f, true, 10, 10f),
                (100, 9f, 1, 0.25f, true, 9, 0f),
                (100, 20f, 2, 0.51f, true, 20, 0f),
                (100, 20f, 2, 0.50f, true, 20, 20f),
                (100, 19f, 2, 0.50f, true, 19, 0f),
                (100, 30f, 3, 0.81f, true, 30, 0f),
                (100, 30f, 3, 0.80f, true, 30, 30f),
                (100, 29f, 3, 0.80f, true, 29, 0f)
            };

        [Test]
        public void TryRecruit_LuckyIndustrialist_SavePowerOnLuck(
            [ValueSource(nameof(LuckyIndustrialistCases))]
            (int percent, float planetPower, int level, float substitudeRandomRange, bool expectedResult,
                int expectedArmyPower, float expectedPlanetRemainingPower) testCase)
        {
            // arrange
            var planet = CreatePlanet(testCase.planetPower, Faction.Green,
                (PerkId.LuckyIndustrialist, testCase.level, PerkType.Epic));

            SubstitutiveRandom.SubstituteNextRange(testCase.substitudeRandomRange);

            // act
            planet.ExecuteProductionCycle();
            var recruited = planet.TryRecruit(new Percentage(testCase.percent), out var armyPower);

            // assert
            Assert.AreEqual(testCase.expectedResult, recruited);
            Assert.AreEqual(testCase.expectedArmyPower, armyPower);
            Assert.AreEqual(testCase.expectedPlanetRemainingPower, planet.Power);
        }

        #endregion
    }
}