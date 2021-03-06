using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.FiniteStateMachine.Tools;

namespace Kugushev.Scripts.Common.FiniteStateMachine
{
    public class StateMachine
    {
        private readonly IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> _transitions;
        private IState _currentState;

        public StateMachine(IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> transitions)
        {
            _transitions = transitions;
            _currentState = EntryState.Instance;
        }

        public async UniTask UpdateAsync(Func<float> deltaTimeProvider)
        {
            if (_transitions.TryGetValue(_currentState, out var transitions))
                foreach (var (transition, targetState) in transitions)
                    if (transition.ToTransition)
                        await SetState(targetState);

            // by reason of async SetState this OnUpdate execution might be in the another frame than tha start of this method
            var deltaTime = deltaTimeProvider();
            _currentState.OnUpdate(deltaTime);
        }

        public async UniTask SetState(IState state)
        {
            await _currentState.OnExitAsync();
            _currentState = null;

            _currentState = state;
            await _currentState.OnEnterAsync();
        }
    }
}