using System.Collections.Generic;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public class Transitions : Dictionary<IState, IReadOnlyList<ITransitionRecord>>
    {
    }
}