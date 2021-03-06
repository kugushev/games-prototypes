namespace Kugushev.Scripts.Common.FiniteStateMachine.Tools
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