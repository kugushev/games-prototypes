using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Tests.Integration.Utils
{
    public class SingletonTransition : ITransition
    {
        public static SingletonTransition Instance { get; } = new SingletonTransition();

        private SingletonTransition()
        {
        }

        public bool ToTransition { get; set; }

        public void Reset() => ToTransition = false;
    }
}