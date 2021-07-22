using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.Managers
{
    public class GameModeManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameRoot;

        private string _additiveScene;

        public async UniTask ToGameAsync()
        {
            if (_additiveScene == null)
            {
                Debug.Log("Game mode is already active");
                return;
            }

            await SceneManager.UnloadSceneAsync(_additiveScene);
            gameRoot.SetActive(true);
            _additiveScene = null;
        }

        public UniTask ToCityAsync()
        {
            gameRoot.SetActive(false);
            return LoadAdditive("CityScene");
        }

        public UniTask ToBattleAsync()
        {
            gameRoot.SetActive(false);
            return LoadAdditive("BattleScene");
        }

        private async UniTask LoadAdditive(string sceneName)
        {
            _additiveScene = sceneName;
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            var scene = SceneManager.GetSceneByName(sceneName);
            bool loaded = SceneManager.SetActiveScene(scene);
            if (!loaded)
                Debug.LogError($"Scene {sceneName} isn't loaded");
        }
    }
}