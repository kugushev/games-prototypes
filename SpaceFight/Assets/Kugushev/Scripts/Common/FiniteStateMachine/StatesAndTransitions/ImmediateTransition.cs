namespace Kugushev.Scripts.Common.FiniteStateMachine.StatesAndTransitions
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