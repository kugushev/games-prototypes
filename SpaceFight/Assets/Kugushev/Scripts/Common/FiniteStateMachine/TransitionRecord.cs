namespace Kugushev.Scripts.Common.FiniteStateMachine
{
    public readonly struct TransitionRecord
    {
        public TransitionRecord(ITransition transition, IState target)
        {
            Transition = transition;
            Target = target;
        }

        public ITransition Transition { get; }
        public IState Target { get; }

        public void Deconstruct(out ITransition transition, out IState target)
        {
            transition = Transition;
            target = Target;
        }
    }
}