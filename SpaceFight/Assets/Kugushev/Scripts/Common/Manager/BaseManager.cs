using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.Common.Manager
{
    public abstract class BaseManager<TRootModel> : MonoBehaviour
    {
        private StateMachine _stateMachine;

        private void Awake()
        {
            var rootModel = InitRootModel();

            var transitions = ComposeStateMachine(rootModel);
            _stateMachine = new StateMachine(transitions);
        }

        private void Start()
        {
            StartCoroutine(Loop().ToCoroutine());
        }

        protected abstract TRootModel InitRootModel();

        protected abstract IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            TRootModel rootModel);

        private async UniTask Loop()
        {
            while (true)
            {
                await _stateMachine.UpdateAsync(() => Time.deltaTime);
                await UniTask.NextFrame();
            }
        }
    }
}