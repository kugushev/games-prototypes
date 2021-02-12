using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
            yield return new WaitForSeconds(30);
        }
    }
}