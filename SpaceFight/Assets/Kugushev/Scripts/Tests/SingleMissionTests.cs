using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.Tests.Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests
{
    public class SingleMissionTests
    {
        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_RandomSeed()
        {
            int seed = DateTime.UtcNow.Millisecond;
            Debug.Log($"Seed {seed}");

            return RunFightWithSeed(seed);
        }

        [UnityTest]
        public IEnumerator Fight_SimpleFight() => RunFightWithSeed(42);

        [UnityTest]
        [Timeout(5 * 60 * 1000)]
        public IEnumerator Fight_ArmiesFight() => RunFightWithSeed(268);

        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_FlyAroundSun() => RunFightWithSeed(193);
        
        [UnityTest]
        public IEnumerator Fight_Backstabbers() => RunFightWithSeed(540);
        
        [UnityTest]
        public IEnumerator Fight_LongPath() => RunFightWithSeed(950);

        private static IEnumerator RunFightWithSeed(int seed, [CallerMemberName] string caller = null)
        {
            Debug.Log($"Start test {caller}");
            TestMissionController.Seed = seed;
            SceneManager.LoadScene("TestingScene");
            yield return new WaitForSeconds(1); // to allow setup Test Scene 
            yield return  new WaitUntil(() => TestMissionController.MissionFinished);
        }
    }
}