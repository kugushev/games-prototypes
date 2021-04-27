using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public class StateMachine
    {
        private readonly IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> _transitions;
        private IUnparameterizedState _currentState;

        public StateMachine(IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> transitions)
        {
            ResetTransitions(transitions);
            _transitions = transitions;

            _currentState = EntryState.Instance;
        }

        public async UniTask UpdateAsync(Func<float> deltaTimeProvider)
        {
            if (_transitions.TryGetValue(_currentState, out var transitions))
                foreach (var (transition, targetState) in transitions)
                    if (transition.ToTransition)
                    {
                        if (transition is IReusableTransition reusableTransition)
                            reusableTransition.Reset();
                        await SetState(targetState);
                    }

            // by reason of async SetState this OnUpdate execution might be in the another frame than tha start of this method
            var deltaTime = deltaTimeProvider();
            _currentState.OnUpdate(deltaTime);
        }

        public async UniTask DisposeAsync()
        {
            await _currentState.OnExitAsync();
        }

        private async UniTask SetState(IUnparameterizedState state)
        {
            // todo: refactor with lifetimes
            /*
            await _currentState.OnExitAsync(); 
            _currentState = state;
            await _currentState.OnEnterAsync();
            
            unload previous scene
                now we can do everything with previous lifetime
            
                init new state
                    Start Lifetime
            
            use dispatching of transition to pass parameters
            
                dispose previous lifetime
            
            load next scene
            
            
            ----- impl -----
             
            var previous = _currentState;
            var next = state;
            
            var parameters = await previous.OnExitAsync();
            
            using(parameters)
                next.Setup(parameters);
            
            _currentState = next;
            
            previous.TearDown();
            
            next.OnEnterAsync();
             */
            
            await _currentState.OnExitAsync();
            _currentState = state;
            await _currentState.OnEnterAsync();
        }

        private static void ResetTransitions(IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> transitions)
        {
            foreach (var pair in transitions)
            foreach (var (transition, _) in pair.Value)
                if (transition is IReusableTransition reusableTransition)
                    reusableTransition.Reset();
        }
    }
}