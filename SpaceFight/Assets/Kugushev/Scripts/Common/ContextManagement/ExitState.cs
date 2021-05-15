#nullable disable
using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;

namespace Kugushev.Scripts.Common.ContextManagement
{
    public class ExitState<T> : IParameterizedState<T>, IParameterizedTransition<T>, IReusableTransition
    {
        private bool _entered;
        private T _parameters;

        UniTask IParameterizedState<T>.OnEnterAsync(T parameters)
        {
            _entered = true;
            _parameters = parameters;
            return UniTask.CompletedTask;
        }

        UniTask IState.OnExitAsync() => UniTask.CompletedTask;

        T IParameterizedTransition<T>.ExtractParameters() => _parameters;

        bool ITransition.ToTransition => _entered;

        void IReusableTransition.Reset()
        {
            _entered = false;
            if (_parameters is IDisposable disposable)
                disposable.Dispose();
            _parameters = default;
        }
    }
}
#nullable enable