using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Common.Modes
{
    public abstract class ParameterizedSceneLoadingState<T> : IParameterizedState<T>
    {
        private readonly string _sceneName;
        private readonly bool _setActive;
        private readonly ParametersPipeline<T> _parametersPipeline;

        protected ParameterizedSceneLoadingState(string sceneName, bool setActive, ParametersPipeline<T> parametersPipeline)
        {
            _sceneName = sceneName;
            _setActive = setActive;
            _parametersPipeline = parametersPipeline;
        }

        public async UniTask OnEnterAsync(T parameters)
        {
            _parametersPipeline.Push(parameters);

            await SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            if (_setActive)
            {
                var scene = SceneManager.GetSceneByName(_sceneName);
                bool loaded = SceneManager.SetActiveScene(scene);
                if (!loaded)
                    Debug.LogError($"{_sceneName} scene isn't loaded");
            }
        }

        public UniTask OnEnterAsync()
        {
            throw new NotSupportedException();
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