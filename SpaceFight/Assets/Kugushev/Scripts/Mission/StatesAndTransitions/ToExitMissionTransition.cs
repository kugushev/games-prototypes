using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class ToExitMissionTransition: ITransition
    {
        public bool ToTransition => false;
    }
}