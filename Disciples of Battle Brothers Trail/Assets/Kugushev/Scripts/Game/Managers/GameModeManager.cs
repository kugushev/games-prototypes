using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Enums;
using Kugushev.Scripts.Common.Exceptions;
using Kugushev.Scripts.Game.Models.CityInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Managers
{
    public class GameModeManager
    {
        private string _additiveScene;
        private object _parameter;

        public GameMode Current { get; private set; } = GameMode.Game;

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

        public UniTask ToCityAsync(CityWorldItem currentCityWorldItem)
        {
            PushParameter(currentCityWorldItem);
            Current = GameMode.City;
            return LoadAdditive("CityScene");
        }

        public UniTask ToBattleAsync()
        {
            Current = GameMode.Battle;
            return LoadAdditive("BattleScene");
        }

        public T PopParameter<T>() where T : class
        {
            if (_parameter == null)
                throw new TheGameException("Parameter is null");

            if (_parameter is T p)
            {
                _parameter = null;
                return p;
            }

            throw new TheGameException($"Wrong type {_parameter}, expected {typeof(T)}");
        }

        private void PushParameter(object parameter)
        {
            if (_parameter != null)
                throw new TheGameException("Parameter is already specified");
            _parameter = parameter;
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