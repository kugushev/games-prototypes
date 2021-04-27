using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Common.Modes
{
#nullable disable
    public class SignaledTransition<T> : IParameterizedTransition<T>
    {
        private T _signal;
        private bool _signaled;

        public void Signalise(T signal)
        {
            _signaled = true;
            _signal = signal;
        }

        bool ITransition.ToTransition => _signaled;

        T IParameterizedTransition<T>.ExtractParameters() => _signal;

        void IReusableTransition.Reset()
        {
            _signaled = false;
            if (_signal is ISelfDespawning selfDespawning)
                selfDespawning.DespawnSelf();
            _signal = default;
        }
    }

#nullable enable
}