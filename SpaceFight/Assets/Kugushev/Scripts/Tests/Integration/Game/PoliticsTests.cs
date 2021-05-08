using System.Collections;
using Kugushev.Scripts.App.Core.ValueObjects;
using Kugushev.Scripts.Tests.Integration.Game.Setup;
using Kugushev.Scripts.Tests.Integration.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Kugushev.Scripts.Tests.Integration.Game
{
    [TestFixture]
    public class PoliticsTests
    {
        [UnityTest]
        [Timeout(10 * 60 * 1000)]
        public IEnumerator RunCheck()
        {
            // todo: СУКАБЛЯ!!!! НЕ ТУПИ!!
            // Интриги будут добавляться через сигналы из миссий. А не как как возвращаемые значения. 
            // Так что в тесте надо просто начать сигналить и проверить что все корректно отображает
            // Анализировать UI будет вообще по тупому: по имени контрола GameObject.Find()


            // todo: ОЧЕНЬ ВАЖНО!!! Как сделать подконтект для Game, так чтобы можно было там использовать сигналы?
            // решение: сделать GameObject context и описать в найтройках Parent как Game.
            // Под ним можно запускать тесты, котоыре будут вызывать сигналы

            SingletonTransition<GameParameters>.Instance.SetValue(new GameParameters(42));

            SceneManager.LoadScene("PoliticsTestingManagementScene");
            
            yield return new WaitUntil(() => false);
        }
    }
}