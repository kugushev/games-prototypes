using System.Collections;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;
using Kugushev.Scripts.Tests.Unit.Stubs;
using Kugushev.Scripts.Tests.Unit.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Unit
{
    public class ArmyTests
    {
        private const float ArmyPower = 50f;
        private const float DeltaTime = GameplayConstants.FightRoundDelay + 1f;

        [Test]
        public void NextStep_FightingStatus_NoAchievements_DamageUnifiedDamage()
        {
            // arrange
            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties();

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
            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties(
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
            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties(
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
            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties(
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
            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties(
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

            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties(
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
            [ValueSource(nameof(BrawlerAndMoskaCases))] (int level, float power, float expected) testCase)
        {
            // arrange
            var (_, fleetProperties) = PlayerPropertiesHelper.GetPlayerProperties(
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

        private static Army CreateArmy(FleetProperties fleetProperties, Faction faction, float power = ArmyPower)
        {
            const float magicNum = 42f;

            var objectsPool = ScriptableObject.CreateInstance<ObjectsPool>();
            var eventsCollector = ScriptableObject.CreateInstance<MissionEventsCollector>();

            var targetPlanet = objectsPool.GetObject<Planet, Planet.State>(default);
            var order = objectsPool.GetObject<Order, Order.State>(new Order.State(targetPlanet, new Percentage(1f)));
            order.RegisterMovement(Vector3.zero);

            var army = new Army(objectsPool);
            army.SetState(new Army.State(order, magicNum, magicNum, faction, power, fleetProperties, eventsCollector));
            return army;
        }
    }
}