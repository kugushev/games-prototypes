using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.StatesAndTransitions
{
    public abstract class BaseSceneLoadingState<TModel> : BaseState<TModel>
    {
        private readonly string _sceneName;
        private readonly bool _setActive;

        protected BaseSceneLoadingState(TModel model, string sceneName, bool setActive) : base(model)
        {
            _sceneName = sceneName;
            _setActive = setActive;
        }

        protected abstract void AssertModel();

        protected virtual void OnEnterBeforeLoadScene()
        {
        }

        public sealed override async UniTask OnEnterAsync()
        {
            AssertModel();
            OnEnterBeforeLoadScene();

            await SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            if (_setActive)
            {
                var scene = SceneManager.GetSceneByName(_sceneName);
                bool loaded = SceneManager.SetActiveScene(scene);
                if (!loaded)
                    Debug.LogError("Main Menu Scene isn't loaded");
            }
        }

        protected virtual void OnExitBeforeUnloadScene()
        {
        }

        public sealed override async UniTask OnExitAsync()
        {
            OnExitBeforeUnloadScene();
            await SceneManager.UnloadSceneAsync(_sceneName);
        }
    }
}