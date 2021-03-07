using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class ToExitMissionTransition: ITransition
    {
        private readonly MissionModel _model;

        public ToExitMissionTransition(MissionModel model)
        {
            _model = model;
        }

        public bool ToTransition => _model.ReadyToFinish;
    }
}