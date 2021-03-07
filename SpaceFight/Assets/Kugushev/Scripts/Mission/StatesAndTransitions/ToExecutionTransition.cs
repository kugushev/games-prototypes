using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class ToExecutionTransition: ITransition
    {
        private readonly MissionModel _missionModel;
        public ToExecutionTransition(MissionModel missionModel) => _missionModel = missionModel;
        public bool ToTransition => _missionModel.ReadyToExecute;
    }
}