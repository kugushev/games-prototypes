using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Tests.Integration.Mission.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests.Integration.Mission
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

        private static IEnumerator RunExecutionWithSeed(int seed, [CallerMemberName] string caller = "")
        {
            Debug.Log($"Start test {caller}");

            SingletonState.Instance.Reset();

            var pool = ScriptableObject.CreateInstance<ObjectsPool>();

            BaseMissionTestingManager.MissionInfo =
                new MissionParameters(
                    new MissionInfo(seed, Difficulty.Normal, ScriptableObject.CreateInstance<Intrigue>()),
                    pool.GetObject<PlayerPerks, PlayerPerks.State>(new PlayerPerks.State(PerkIdHelper.AllPerks)));
            SceneManager.LoadScene("MissionExecutionTestingManagementScene");

            yield return new WaitUntil(() => SingletonState.Instance.Entered);
        }
    }
}