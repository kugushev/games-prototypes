using System;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;

namespace Kugushev.Scripts.Common.ContextManagement
{
#nullable disable
    public class SignaledTransition<T> : IParameterizedTransition<T>, IReusableTransition
    {
        private T _parameters;
        private bool _signaled;

        public void Signalise(T signal)
        {
            _signaled = true;
            _parameters = signal;
        }

        bool ITransition.ToTransition => _signaled;

        T IParameterizedTransition<T>.ExtractParameters() => _parameters;

        void IReusableTransition.Reset()
        {
            _signaled = false;
            if (_parameters is IDisposable disposable)
                disposable.Dispose();
            _parameters = default;
        }
    }

#nullable enable
}