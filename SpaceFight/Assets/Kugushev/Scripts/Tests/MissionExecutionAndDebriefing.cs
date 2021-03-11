using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Tests.Setup;
using Kugushev.Scripts.Tests.Setup.Abstractions;
using Kugushev.Scripts.Tests.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests
{
    public class MissionExecutionAndDebriefing
    {
        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_RandomSeed()
        {
            int seed = DateTime.UtcNow.Millisecond;
            Debug.Log($"Seed {seed}");

            yield return RunExecutionWithSeed(seed);

            CollectionAssert.IsNotEmpty(BaseExecutionTestingManager.MissionModel.DebriefingSummary.AllAchievements);

            Debug.Log("Achievements");
            foreach (var achievement in BaseExecutionTestingManager.MissionModel.DebriefingSummary.AllAchievements)
                Debug.Log(achievement);
        }

        [UnityTest]
        public IEnumerator Fight_SimpleFight()
        {
            yield return RunExecutionWithSeed(42);
            var allAchievements = BaseExecutionTestingManager.MissionModel.DebriefingSummary.AllAchievements;

            Assert.That(allAchievements,
                Has.Exactly(1).Matches<AbstractAchievement>(a => a.Info.Id == AchievementId.Invader));
        }


        private static IEnumerator RunExecutionWithSeed(int seed, [CallerMemberName] string caller = null)
        {
            Debug.Log($"Start test {caller}");

            SingletonState.Instance.Reset();
            
            BaseExecutionTestingManager.Seed = seed;
            SceneManager.LoadScene("MissionExecutionAndDebriefingTestingManagementScene");
            
            // we need to wait for the mission finished to init BaseExecutionTestingManager.MissionModel.DebriefingSummary
            yield return new WaitUntil(() => SingletonState.Instance.Entered); 
            yield return new WaitUntil(() => BaseExecutionTestingManager.MissionModel.DebriefingSummary != null);
        }
    }
}