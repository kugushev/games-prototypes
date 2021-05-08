#nullable disable
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;

namespace Kugushev.Scripts.Tests.Integration.Game.Setup
{
    public class SingletonTransition<T> : IParameterizedTransition<T>
    {
        public static readonly SingletonTransition<T> Instance = new SingletonTransition<T>();
        private T _parameter;

        private SingletonTransition()
        {
        }

        public void SetValue(T parameter) => _parameter = parameter;
        bool ITransition.ToTransition => true;
        T IParameterizedTransition<T>.ExtractParameters() => _parameter;
    }
}

#nullable enable