using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.ContextManagement
{
    public class UnparameterizedSceneLoadingState : IUnparameterizedState
    {
        private readonly string _sceneName;
        private readonly bool _setActive;


        protected UnparameterizedSceneLoadingState(string sceneName, bool setActive)
        {
            _sceneName = sceneName;
            _setActive = setActive;
        }

        public async UniTask OnEnterAsync()
        {
            await SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            if (_setActive)
            {
                var scene = SceneManager.GetSceneByName(_sceneName);
                bool loaded = SceneManager.SetActiveScene(scene);
                if (!loaded)
                    Debug.LogError($"{_sceneName} scene isn't loaded");
            }
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public async UniTask OnExitAsync()
        {
            await SceneManager.UnloadSceneAsync(_sceneName);
        }
    }
}