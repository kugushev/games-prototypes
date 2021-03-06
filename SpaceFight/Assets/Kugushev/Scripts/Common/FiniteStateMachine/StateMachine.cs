using System.Collections.Generic;
using UnityEngine;

namespace Kugushev.Scripts.Common.FiniteStateMachine
{
    public class StateMachine
    {
        private readonly IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> _transitions;
        private IState _currentState;

        public StateMachine(IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> transitions)
        {
            _transitions = transitions;
        }

        public void Update(float deltaTime)
        {
            if (_transitions.TryGetValue(_currentState, out var transitions))
                foreach (var (transition, targetState) in transitions)
                    if (transition.ToTransition)
                        SetState(targetState);

            _currentState?.OnUpdate(deltaTime);
        }

        public void SetState(IState state)
        {
            _currentState?.OnExitAsync();
            _currentState = null;

            _currentState = state;
            _currentState.OnEnterAsync();
        }
    }
}