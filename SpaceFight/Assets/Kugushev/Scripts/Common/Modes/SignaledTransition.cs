using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Common.Modes
{
#nullable disable
    public class SignaledTransition<T> : IParameterizedTransition<T>
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
            if (_parameters is ISelfDespawning selfDespawning)
                selfDespawning.DespawnSelf();
            _parameters = default;
        }
    }

#nullable enable
}