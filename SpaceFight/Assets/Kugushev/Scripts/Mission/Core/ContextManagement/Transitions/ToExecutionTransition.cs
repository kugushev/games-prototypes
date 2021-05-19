using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Mission.Core.ContextManagement.Transitions
{
    public class ToExecutionTransition : ITransition, IReusableTransition
    {
        public bool ToTransition { get; set; }

        void IReusableTransition.Reset()
        {
            ToTransition = false;
        }
    }
}