using System;
using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public class TransitionRecord : ITransitionRecord
    {
        private readonly ITransition _transition;
        private readonly IUnparameterizedState _target;

        public TransitionRecord(ITransition transition, IUnparameterizedState target)
        {
            _transition = transition;
            _target = target;
        }

        public IState Target => _target;
        public bool ToTransition => _transition.ToTransition;

        public UniTask EnterState() => _target.OnEnterAsync();

        public void ResetTransition()
        {
            if (_transition is IReusableTransition reusableTransition)
                reusableTransition.Reset();
        }
    }

    // todo: add pooling
    public class TransitionRecord<T> : ITransitionRecord
    {
        private readonly IParameterizedTransition<T> _transition;
        private readonly IParameterizedState<T> _target;

        public TransitionRecord(IParameterizedTransition<T> transition, IParameterizedState<T> target)
        {
            _transition = transition;
            _target = target;
        }

        public IState Target => _target;
        public bool ToTransition => _transition.ToTransition;

        public UniTask EnterState()
        {
            var parameters = _transition.ExtractParameters();
            return _target.OnEnterAsync(parameters);
        }

        public void ResetTransition()
        {
            if (_transition is IReusableTransition reusableTransition)
                reusableTransition.Reset();
        }
    }
}