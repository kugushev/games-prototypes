using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.Tests.Setup;
using Kugushev.Scripts.Tests.Setup.Abstractions;
using Kugushev.Scripts.Tests.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests
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
        public IEnumerator Fight_SimpleFight() => RunExecutionWithSeed(42);

        [UnityTest]
        [Timeout(5 * 60 * 1000)]
        public IEnumerator Fight_ArmiesFight() => RunExecutionWithSeed(268);

        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_FlyAroundSun() => RunExecutionWithSeed(193);

        [UnityTest]
        public IEnumerator Fight_Backstabbers() => RunExecutionWithSeed(540);

        [UnityTest]
        public IEnumerator Fight_LongPath() => RunExecutionWithSeed(950);

        private static IEnumerator RunExecutionWithSeed(int seed, [CallerMemberName] string caller = null)
        {
            Debug.Log($"Start test {caller}");
            
            SingletonState.Instance.Reset();

            BaseExecutionTestingManager.Seed = seed;
            SceneManager.LoadScene("MissionExecutionTestingManagementScene");

            yield return new WaitUntil(() => SingletonState.Instance.Entered);
        }
    }
}