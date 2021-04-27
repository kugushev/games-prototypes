using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Common.Modes
{
    public abstract class AbstractModeManager : MonoBehaviour //, ITickable
    {
        private ParameterizedStateMachine _stateMachine = default!;

        protected EntryState Entry => EntryState.Instance;

        protected ImmediateTransition Immediate => ImmediateTransition.Instance;

        protected abstract Transitions ComposeStateMachine();

        protected virtual void OnStart()
        {
        }

        private void Awake()
        {
            var transitions = ComposeStateMachine();
            _stateMachine = new ParameterizedStateMachine(transitions);
        }

        private void Start()
        {
            OnStart();
            StartCoroutine(Loop().ToCoroutine());
        }

        private void OnDestroy()
        {
            // because we can't run coroutine on destroy
#pragma warning disable 4014
            _stateMachine.DisposeAsync();
#pragma warning restore 4014
        }

        private async UniTask Loop()
        {
            while (true)
            {
                await _stateMachine.UpdateAsync(() => Time.deltaTime);
                await UniTask.NextFrame();
            }

            // ReSharper disable once FunctionNeverReturns
        }

        // private UniTask? _updateTask;
        //
        // public void Tick()
        // {
        //     if (_updateTask != null && _updateTask.Value.Status == UniTaskStatus.Pending)
        //         return;
        //
        //     _updateTask = _stateMachine.UpdateAsync(() => Time.deltaTime);
        // }
    }
}