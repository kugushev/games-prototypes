using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public class ParameterizedStateMachine
    {
        private readonly Transitions _transitions;
        private IState _currentState;

        public ParameterizedStateMachine(Transitions transitions)
        {
            ResetTransitions(transitions);
            _transitions = transitions;

            _currentState = EntryState.Instance;
        }

        public async UniTask UpdateAsync(Func<float> deltaTimeProvider)
        {
            if (_transitions.TryGetValue(_currentState, out var transitions))
                foreach (var record in transitions)
                    if (record.ToTransition)
                    {
                        await SetState(record);
                        record.ResetTransition();
                    }

            // by reason of async SetState this OnUpdate execution might be in the another frame than tha start of this method
            var deltaTime = deltaTimeProvider();
            _currentState.OnUpdate(deltaTime);
        }

        public async UniTask DisposeAsync()
        {
            await _currentState.OnExitAsync();
        }

        private async UniTask SetState(ITransitionRecord record)
        {
            await _currentState.OnExitAsync();
            _currentState = record.Target;
            await record.EnterState();
        }

        private static void ResetTransitions(Transitions transitions)
        {
            foreach (var pair in transitions)
            foreach (var record in pair.Value)
                record.ResetTransition();
        }
    }
}