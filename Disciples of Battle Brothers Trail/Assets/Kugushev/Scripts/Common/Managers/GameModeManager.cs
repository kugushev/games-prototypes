using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.Managers
{
    public class GameModeManager
    {
        // todo: use Zenject
        public static GameModeManager Instance { get; } = new GameModeManager();

        public GameMode Current { get; private set; } = GameMode.Game;

        private string _additiveScene;

        public async UniTask ToGameAsync()
        {
            if (_additiveScene == null)
            {
                Debug.Log("Game mode is already active");
                return;
            }

            await SceneManager.UnloadSceneAsync(_additiveScene);
            _additiveScene = null;
            Current = GameMode.Game;
        }

        public UniTask ToCityAsync()
        {
            Current = GameMode.City;
            return LoadAdditive("CityScene");
        }

        public UniTask ToBattleAsync()
        {
            Current = GameMode.Battle;
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