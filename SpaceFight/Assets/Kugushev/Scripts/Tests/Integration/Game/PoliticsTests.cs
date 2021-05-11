using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Tests.Integration.Game.Setup;
using NUnit.Framework;
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
            SingletonTransition<GameParameters>.Instance.SetValue(new GameParameters(42));

            yield return Exec().ToCoroutine();
        }

        private async UniTask Exec()
        {
            await SceneManager.LoadSceneAsync("AppStubScene");

            await UniTask.WaitForEndOfFrame();
            
            //await UniTask.Delay(TimeSpan.FromSeconds(1));

            await SceneManager.LoadSceneAsync("PoliticsTestingManagementScene", LoadSceneMode.Additive);

            await UniTask.WaitWhile(() => true);
        }
    }
}