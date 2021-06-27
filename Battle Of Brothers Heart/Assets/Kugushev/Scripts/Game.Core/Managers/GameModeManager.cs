using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class GameModeManager
    {
        private string currentScene = "CampaingScene";

        public UniTask SwitchToCampaignModeAsync()
        {
            return SwitchSceneAsync("CampaingScene");
        }

        public UniTask SwitchToCityModeAsync()
        {
            return SwitchSceneAsync("CityScene");
        }

        public UniTask SwitchToBattleModeAsync()
        {
            return SwitchSceneAsync("BattleScene");
        }

        private async UniTask SwitchSceneAsync(string sceneName)
        {
            await SceneManager.UnloadSceneAsync(currentScene);
            currentScene = sceneName;

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            var scene = SceneManager.GetSceneByName(sceneName);
            bool loaded = SceneManager.SetActiveScene(scene);
            if (!loaded)
                Debug.LogError("Main Menu Scene isn't loaded");
        }
    }
}