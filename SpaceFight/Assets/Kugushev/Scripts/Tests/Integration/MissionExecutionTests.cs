using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Tests.Integration.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests.Integration
{
    public class MissionExecutionTests
    {
        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_RandomSeed()
        {
            int seed = DateTime.UtcNow.Millisecond;
            Debug.Log($"Seed {seed}");

            return RunExecutionWithSeed(seed);
        }

        [UnityTest]
        public IEnumerator Fight_3PlanetsFight() => RunExecutionWithSeed(Seeds.ThreePlanets);

        [UnityTest]
        [Timeout(5 * 60 * 1000)]
        public IEnumerator Fight_ArmiesFight() => RunExecutionWithSeed(Seeds.ArmiesFight);

        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_FlyAroundSun() => RunExecutionWithSeed(Seeds.FlyAroundSun);

        [UnityTest]
        public IEnumerator Fight_Backstabbers() => RunExecutionWithSeed(540);

        [UnityTest]
        public IEnumerator Fight_LongPath() => RunExecutionWithSeed(950);

        private static IEnumerator RunExecutionWithSeed(int seed, [CallerMemberName] string caller = null)
        {
            Debug.Log($"Start test {caller}");
            
            SingletonState.Instance.Reset();

            BaseExecutionTestingManager.MissionInfo = new MissionInfo(seed, new PlayerAchievements());
            SceneManager.LoadScene("MissionExecutionTestingManagementScene");

            yield return new WaitUntil(() => SingletonState.Instance.Entered);
        }
    }
}