using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Tests.Integration.Setup;
using Kugushev.Scripts.Tests.Integration.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests.Integration
{
    public class MissionBriefingTests
    {
        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Fight_RandomSeed()
        {
            int seed = DateTime.UtcNow.Millisecond;
            Debug.Log($"Seed {seed}");

            return RunExecutionWithSeed(seed);
        }

        // todo: in real test rotate and verify if no collisions between planets

        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator Briefing_ForManualTest() => RunExecutionWithSeed(83);

        private static IEnumerator RunExecutionWithSeed(int seed, [CallerMemberName] string caller = null)
        {
            Debug.Log($"Start test {caller}");

            MissionBriefingTestingManager.Seed = seed;
            SceneManager.LoadScene("MissionBriefingTestingManagementScene");

            yield return new WaitUntil(() => SingletonState.Instance.Entered);
        }
    }
}