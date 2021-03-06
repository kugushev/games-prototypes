using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Common.StatesAndTransitions
{
    public class ImmediateTransition : ITransition
    {
        public static ImmediateTransition Instance { get; } = new ImmediateTransition();

        private ImmediateTransition()
        {
        }

        public bool ToTransition => true;
    }
}