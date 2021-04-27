using System.Collections.Generic;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public class Transitions : Dictionary<IState, IReadOnlyList<ITransitionRecord>>
    {
        public static TransitionRecord<T> Record<T>(IParameterizedTransition<T> transition,
            IParameterizedState<T> target) =>
            new TransitionRecord<T>(transition, target);
    }
}