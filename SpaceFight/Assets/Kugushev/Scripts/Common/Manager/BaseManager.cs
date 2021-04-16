using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.Common.Manager
{
    public abstract class BaseManager<TRootModel> : MonoBehaviour
        where TRootModel : class, IDisposable
    {
        private StateMachine? _stateMachine;
        private TRootModel? _rootModel;

        protected TRootModel RootModel
        {
            get
            {
                Asserting.NotNull(_rootModel);
                return _rootModel;
            }
        }

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
            if (_stateMachine is { })
                // because we can't run couroutine on destory
#pragma warning disable 4014
                _stateMachine.DisposeAsync();
#pragma warning restore 4014
            Dispose();

            if (_rootModel is { })
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
            Asserting.NotNull(_stateMachine);
            while (true)
            {
                await _stateMachine.UpdateAsync(() => Time.deltaTime);
                await UniTask.NextFrame();
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}