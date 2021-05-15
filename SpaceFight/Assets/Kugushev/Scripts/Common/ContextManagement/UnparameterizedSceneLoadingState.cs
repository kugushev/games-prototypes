using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.ContextManagement
{
    public abstract class UnparameterizedSceneLoadingState : IUnparameterizedState
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
            OnEnterBeforeLoadScene();
            await SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            if (_setActive)
            {
                var scene = SceneManager.GetSceneByName(_sceneName);
                bool loaded = SceneManager.SetActiveScene(scene);
                if (!loaded)
                    Debug.LogError($"{_sceneName} scene isn't loaded");
            }

            OnEnterAfterLoadScene();
        }

        public async UniTask OnExitAsync()
        {
            OnExitBeforeUnloadScene();
            await SceneManager.UnloadSceneAsync(_sceneName);
        }

        protected virtual void OnEnterBeforeLoadScene()
        {
        }

        protected virtual void OnEnterAfterLoadScene()
        {
        }

        protected virtual void OnExitBeforeUnloadScene()
        {
        }
    }
}