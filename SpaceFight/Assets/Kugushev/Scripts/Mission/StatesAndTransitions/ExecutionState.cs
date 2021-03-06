using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    internal class ExecutionState : BaseSceneLoadingState<MissionModel>
    {
        public ExecutionState(MissionModel model)
            : base(model, "", true)
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.Green.Commander.AssignFleet(Model.Green.Fleet, Model.Green.Faction);
            Model.Red.Commander.AssignFleet(Model.Red.Fleet, Model.Red.Faction);
        }
    }
}