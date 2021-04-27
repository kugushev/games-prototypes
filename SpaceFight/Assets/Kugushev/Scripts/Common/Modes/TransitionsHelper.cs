using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;

namespace Kugushev.Scripts.Common.Modes
{
    public static class TransitionsHelper
    {
        public static TransitionRecord TransitTo(this ITransition transition,
            IUnparameterizedState target) =>
            new TransitionRecord(transition, target);

        public static TransitionRecord<T> TransitTo<T>(this IParameterizedTransition<T> transition,
            IParameterizedState<T> target) =>
            new TransitionRecord<T>(transition, target);
        
        
    }
}