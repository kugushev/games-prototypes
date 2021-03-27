using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Campaign.Enums;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Tests.Integration.Mission.Setup;
using Kugushev.Scripts.Tests.Integration.Mission.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests.Integration.Mission
{
    public class MissionExecutionAndDebriefingTests
    {
        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_RandomSeed()
        {
            int seed = DateTime.UtcNow.Millisecond;
            Debug.Log($"Seed {seed}");

            yield return RunExecutionWithSeed(seed, new (AchievementId, int level, AchievementType)[0]);

            CollectionAssert.IsNotEmpty(BaseMissionTestingManager.MissionModel.DebriefingSummary.AllAchievements);

            LogAllAchievements();
        }

        [UnityTest]
        public IEnumerator Fight_3PlanetsFight_NoAchievements_AchieveStartupAndMoskaLvl1()
        {
            yield return RunExecutionWithSeed(Seeds.ThreePlanets, new (AchievementId, int level, AchievementType)[0]);

            LogAllAchievements();

            // assert
            var allAchievements = BaseMissionTestingManager.MissionModel.DebriefingSummary.AllAchievements;
            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Invader));

            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Startup && a.Info.Level == 1));

            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Moska && a.Info.Level == 1));
        }

        [UnityTest]
        public IEnumerator Fight_3PlanetsFight_HasStartupAndMoskaLvl1Achievement_AchieveStartupAndMoskaLvl2()
        {
            yield return RunExecutionWithSeed(Seeds.ThreePlanets, new[]
            {
                (AchievementId.Startup, 1, AchievementType.Epic),
                (AchievementId.Moska, 1, AchievementType.Epic)
            });

            LogAllAchievements();

            // assert
            var allAchievements = BaseMissionTestingManager.MissionModel.DebriefingSummary.AllAchievements;
            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Startup && a.Info.Level == 2));
            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Startup && a.Info.Level == 2));
        }

        [UnityTest]
        public IEnumerator Fight_3PlanetsFight_HasStartupAndMoskaLvl2Achievement_AchieveStartupAndMoskaLvl3()
        {
            yield return RunExecutionWithSeed(Seeds.ThreePlanets, new[]
            {
                (AchievementId.Startup, 2, AchievementType.Epic),
                (AchievementId.Moska, 2, AchievementType.Epic)
            });

            LogAllAchievements();

            // assert
            var allAchievements = BaseMissionTestingManager.MissionModel.DebriefingSummary.AllAchievements;
            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Startup && a.Info.Level == 3));
        }

        [UnityTest]
        public IEnumerator Fight_ArmiesFight_NoAchievements_AchieveKamikazeLvl1()
        {
            MissionExecutionAndDebriefingTestingManager.GreenIsNormal = true;
            MissionExecutionAndDebriefingTestingManager.RedIsNormal = true;

            yield return RunExecutionWithSeed(Seeds.ArmiesFight, new (AchievementId, int level, AchievementType)[0]);

            LogAllAchievements();

            // assert
            var allAchievements = BaseMissionTestingManager.MissionModel.DebriefingSummary.AllAchievements;
            Assert.That(allAchievements, Has.Exactly(1)
                .Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Kamikaze && a.Info.Level == 1));
        }

        private static IEnumerator RunExecutionWithSeed(int seed,
            IEnumerable<(AchievementId, int level, AchievementType)> achievements,
            [CallerMemberName] string caller = null)
        {
            Debug.Log($"Start test {caller}");

            SingletonState.Instance.Reset();

            var achievementsModel = new PlayerAchievements();
            foreach (var (achievementId, level, achievementType) in achievements)
            {
                achievementsModel.AddAchievement(
                    new AchievementInfo(achievementId, level, achievementType, "", "", ""));
            }

            BaseMissionTestingManager.MissionInfo = new MissionParameters(
                new MissionInfo(seed, Difficulty.Normal), 
                achievementsModel);

            SceneManager.LoadScene("MissionExecutionAndDebriefingTestingManagementScene");

            // we need to wait for the mission finished to init BaseExecutionTestingManager.MissionModel.DebriefingSummary
            yield return new WaitUntil(() => SingletonState.Instance.Entered);
            yield return new WaitUntil(() => BaseMissionTestingManager.MissionModel.DebriefingSummary != null);

            MissionExecutionAndDebriefingTestingManager.GreenIsNormal = default;
            MissionExecutionAndDebriefingTestingManager.RedIsNormal = default;
        }

        private static void LogAllAchievements()
        {
            Debug.Log("Achievements");
            foreach (var achievement in BaseMissionTestingManager.MissionModel.DebriefingSummary.AllAchievements)
                Debug.Log(achievement);
        }
    }
}