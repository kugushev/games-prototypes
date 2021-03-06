using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.Utils
{
    public static class SceneManagerHelper
    {
        public static async UniTask LoadAndSetActiveAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(sceneName);
            bool loaded = SceneManager.SetActiveScene(scene);
            if (!loaded)
            {
                Debug.LogError("Main Menu Scene isn't loaded");
            }
        }
    }
}