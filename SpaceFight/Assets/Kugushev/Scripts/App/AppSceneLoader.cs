using Cysharp.Threading.Tasks;
using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common;
using UnityEngine.SceneManagement;
using Zenject;

namespace Kugushev.Scripts.App
{
    public class AppSceneLoader
    {
        private readonly ZenjectSceneLoader _sceneLoader;

        public AppSceneLoader(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async void LoadNewGame(GameModeParameters gameModeParameters)
        {
            const string sceneName = UnityConstants.GameManagementScene;

            var previousScene = SceneManager.GetActiveScene();
            await SceneManager.UnloadSceneAsync(previousScene);

            await _sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive, container =>
            {
                container.Bind<GameModeParameters>()
                    .WithId(CommonConstants.SceneParameters)
                    .FromInstance(gameModeParameters);
            });
        }
    }
}