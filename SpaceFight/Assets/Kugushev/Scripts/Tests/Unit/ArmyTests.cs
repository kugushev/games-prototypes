using System.Collections.Generic;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Tests.Unit.Utils;
using NUnit.Framework;
using static Kugushev.Scripts.Tests.Unit.Utils.TestData;

namespace Kugushev.Scripts.Tests.Unit
{
    public class ArmyTests
    {
        private const float DeltaTime = GameplayConstants.FightRoundDelay + 1f;

        #region NextStep

        #region Fighting

        [Test]
        public void NextStep_FightingStatus_NoAchievements_DamageUnifiedDamage()
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties();

            var army = CreateArmy(fleetProperties, Faction.Green);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(49f, enemy.Power);
        }

        [Test]
        public void NextStep_FightingStatus_BrawlerX1_Damage1d1()
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Brawler, null, AchievementType.Common));

            var army = CreateArmy(fleetProperties, Faction.Green);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(48.9f, enemy.Power);
        }

        [Test]
        public void NextStep_FightingStatus_BrawlerX2_Damage1d2()
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Brawler, null, AchievementType.Common),
                (AchievementId.Brawler, null, AchievementType.Common));

            var army = CreateArmy(fleetProperties, Faction.Green);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(48.8f, enemy.Power);
        }

        [Test]
        public void NextStep_FightingStatus_MoskaLvl1But50Power_DamageUnifiedDamage()
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Moska, 1, AchievementType.Epic));

            var army = CreateArmy(fleetProperties, Faction.Green);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(49f, enemy.Power);
        }

        private static IEnumerable<(int level, float power, float expected)> MoskaCases => new[]
        {
            (1, 19f, 49.3f),
            (2, 9f, 49.6f),
            (3, 4f, 49.9f),
        };

        [Test]
        public void NextStep_FightingStatus_MoskaX_DamageY(
            [ValueSource(nameof(MoskaCases))] (int level, float power, float expected) testCase)
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Moska, testCase.level, AchievementType.Epic));

            var army = CreateArmy(fleetProperties, Faction.Green, testCase.power);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expected, enemy.Power);
        }

        private static IEnumerable<(int level, float power, float expected)> EnemyMoskaCases => new[]
        {
            (1, 19f, 18.3f),
            (2, 9f, 8.6f),
            (3, 4f, 3.9f),
        };

        [Test]
        public void NextStep_FightingStatus_EnemyMoskaX_DamageY(
            [ValueSource(nameof(EnemyMoskaCases))] (int level, float power, float expected) testCase)
        {
            // arrange
            var army = CreateArmy(default, Faction.Green);

            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Moska, testCase.level, AchievementType.Epic));
            var enemy = CreateArmy(fleetProperties, Faction.Red, testCase.power);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expected, enemy.Power);
        }


        private static IEnumerable<(int level, float power, float expected)> BrawlerAndMoskaCases => new[]
        {
            (1, 19f, 49.23f),
            (2, 9f, 49.56f),
            (3, 4f, 49.89f),
        };

        [Test]
        public void NextStep_FightingStatus_BrawlerXAndMoskaY_DamageZ(
            [ValueSource(nameof(BrawlerAndMoskaCases))]
            (int level, float power, float expected) testCase)
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Moska, testCase.level, AchievementType.Epic),
                (AchievementId.Brawler, null, AchievementType.Common));

            var army = CreateArmy(fleetProperties, Faction.Green, testCase.power);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expected, enemy.Power);
        }

        private static IEnumerable<(int level, float power, float expected)> ElephantCases => new[]
        {
            (1, 14f, 49f),
            (1, 15f, 48.5f),
            (2, 24f, 49f),
            (2, 25f, 47.5f),
            (3, 44f, 49f),
            (3, 45f, 45f),
        };

        [Test]
        public void NextStep_FightingStatus_Elephant_DamageIfBigEnough(
            [ValueSource(nameof(ElephantCases))] (int level, float power, float expected) testCase)
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Elephant, testCase.level, AchievementType.Epic));

            var army = CreateArmy(fleetProperties, Faction.Green, testCase.power);
            var enemy = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expected, enemy.Power);
        }

        #endregion

        #region OnSiege

        [Test]
        public void NextStep_OnSiegeStatus_NoAchievements_DamageUnifiedDamage()
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties();

            var planet = CreatePlanet(10f, Faction.Red);

            var army = CreateArmy(fleetProperties, Faction.Green, targetPlanet: planet);

            // act
            planet.ExecuteProductionCycle();

            army.Status = ArmyStatus.OnMatch;
            army.HandlePlanetVisiting(planet);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(9f, planet.Power);
        }

        [Test]
        public void NextStep_OnSiegeStatus_Elephant_DamageIfBigEnough(
            [ValueSource(nameof(ElephantCases))] (int level, float power, float expected) testCase)
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Elephant, testCase.level, AchievementType.Epic));

            var planet = CreatePlanet(50f, Faction.Red);

            var army = CreateArmy(fleetProperties, Faction.Green, testCase.power, planet);

            // act
            planet.ExecuteProductionCycle();

            army.Status = ArmyStatus.OnMatch;
            army.HandlePlanetVisiting(planet);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expected, planet.Power);
        }

        private const float NegotiatorCasesPlanetPower = 20f;

        private static IEnumerable<(int level, float power, Faction expectedPlanetFaction, float? expectedPlanetPower)>
            NegotiatorCases
            => new[]
            {
                (1, 29f, Faction.Neutral, default(float?)),
                (1, 30f, Faction.Green, 30f),
                (2, 34f, Faction.Neutral, default),
                (2, 35f, Faction.Green, 35f + NegotiatorCasesPlanetPower / 2),
                (3, 39f, Faction.Neutral, default),
                (3, 40f, Faction.Green, 40f + NegotiatorCasesPlanetPower),
            };

        [Test]
        public void NextStep_OnSiegeStatus_Negotiator_SurrenderPlanetIfPossible(
            [ValueSource(nameof(NegotiatorCases))]
            (int level, float power, Faction expectedPlanetFaction, float? expectedPlanetPower) testCase)
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Negotiator, testCase.level, AchievementType.Epic));

            var planet = CreatePlanet(0f, Faction.Neutral, power: NegotiatorCasesPlanetPower);

            var army = CreateArmy(fleetProperties, Faction.Green, testCase.power, planet);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandlePlanetVisiting(planet);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expectedPlanetFaction, planet.Faction);
            if (testCase.expectedPlanetPower != null)
                Assert.AreEqual(testCase.expectedPlanetPower.Value, planet.Power);
        }

        private static IEnumerable<(int level, float planetPower, float expectedArmyPower)>
            PlanetIsImpregnable
            => new[]
            {
                (1, 19f + GameplayConstants.UnifiedDamage, 49f),
                (1, 20f + GameplayConstants.UnifiedDamage, 48f),
                (2, 34f + GameplayConstants.UnifiedDamage, 49f),
                (2, 35f + GameplayConstants.UnifiedDamage, 46f),
                (3, 49f + GameplayConstants.UnifiedDamage, 49f),
                (3, 50f + GameplayConstants.UnifiedDamage, 42f),
            };

        [Test]
        public void NextStep_OnSiegeStatus_PlanetIsImpregnable_SufferHighDamage(
            [ValueSource(nameof(PlanetIsImpregnable))]
            (int level, float planetPower, float expectedArmyPower) testCase)
        {
            // arrange
            var planet = CreatePlanet(0f, Faction.Green, testCase.planetPower,
                (AchievementId.Impregnable, testCase.level, AchievementType.Epic));

            var army = CreateArmy(null, Faction.Red, 50f, planet);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandlePlanetVisiting(planet);
            army.NextStep(DeltaTime);

            // assert
            Assert.AreEqual(testCase.expectedArmyPower, army.Power);
        }

        #endregion

        #endregion

        #region SufferFightRound

        private static IEnumerable<(int level, float expected)> KamikazeCases => new[]
        {
            (1, 48f),
            (2, 45f),
            (3, 40f),
        };

        [Test]
        public void SufferFightRound_KamikazeLvlX_2Targets_DealDeathStrikeToOneTarget(
            [ValueSource(nameof(KamikazeCases))] (int level, float expected) testCase)
        {
            // arrange
            var (_, fleetProperties) = PerksHelper.GetPlayerProperties(
                (AchievementId.Kamikaze, testCase.level, AchievementType.Epic));

            var army = CreateArmy(fleetProperties, Faction.Green, 1f);
            var enemy1 = CreateArmy(default, Faction.Red);
            var enemy2 = CreateArmy(default, Faction.Red);

            // act
            army.Status = ArmyStatus.OnMatch;
            army.HandleArmyInteraction(enemy1);
            army.HandleArmyInteraction(enemy2);

            army.SufferFightRound(Faction.Red);

            // assert
            Assert.AreEqual(testCase.expected, enemy1.Power);
            Assert.AreEqual(ArmyPower, enemy2.Power);
        }

        #endregion
    }
}