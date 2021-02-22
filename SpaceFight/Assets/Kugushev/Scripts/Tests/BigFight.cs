using System.Collections;
using Kugushev.Scripts.Presentation.Controllers;
using Kugushev.Scripts.Tests.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests
{
    public class BigFight
    {
        [UnityTest]
        public IEnumerator RunBigFight()
        {
            SceneManager.LoadScene("TestingScene");
            yield return new WaitUntil(() => TestMissionController.MissionFinished);
        }
    }
}