using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.Common.Manager
{
    public abstract class BaseManager<TRootModel> : MonoBehaviour
        where TRootModel : IDisposable
    {
        private StateMachine _stateMachine;
        private TRootModel _rootModel;

        protected TRootModel RootModel => _rootModel;

        private void Awake()
        {
            _rootModel = InitRootModel();

            var transitions = ComposeStateMachine(_rootModel);
            _stateMachine = new StateMachine(transitions);
        }

        private void Start()
        {
            OnStart();
            StartCoroutine(Loop().ToCoroutine());
        }

        private void OnDestroy()
        {
            // there is not way to run coroutine OnDestroy
            _stateMachine?.DisposeAsync();
            Dispose();
            _rootModel.Dispose();
        }

        protected abstract TRootModel InitRootModel();

        protected abstract IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            TRootModel rootModel);

        protected virtual void OnStart()
        {
        }

        protected abstract void Dispose();

        private async UniTask Loop()
        {
            while (true)
            {
                await _stateMachine.UpdateAsync(() => Time.deltaTime);
                await UniTask.NextFrame();
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}