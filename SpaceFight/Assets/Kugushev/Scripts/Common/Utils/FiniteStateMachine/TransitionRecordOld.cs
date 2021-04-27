namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public readonly struct TransitionRecordOld
    {
        public TransitionRecordOld(ITransition transition, IUnparameterizedState target)
        {
            Transition = transition;
            Target = target;
        }

        public ITransition Transition { get; }
        public IUnparameterizedState Target { get; }

        public void Deconstruct(out ITransition transition, out IUnparameterizedState target)
        {
            transition = Transition;
            target = Target;
        }
    }
}