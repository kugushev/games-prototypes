using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    internal class ExecutionState : BaseSceneLoadingState<MissionModel>
    {
        public ExecutionState(MissionModel model)
            : base(model, UnityConstants.Scenes.MissionExecutionScene, true)
        {
        }

        protected override void AssertModel()
        {
            if (Model.PlanetarySystem == null)
                Debug.LogError("PlanetarySystem is not set");

            if (Equals(Model.Green, default(ConflictParty)))
                Debug.LogError("Green is not set");

            if (Equals(Model.Red, default(ConflictParty)))
                Debug.LogError("Green is not set");
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.Green.Commander.AssignFleet(Model.Green.Fleet, Model.Green.Faction);
            Model.Red.Commander.AssignFleet(Model.Red.Fleet, Model.Red.Faction);
        }

        protected override void OnExitBeforeUnloadScene()
        {
            if (!ToDebriefingTransition.IsMissionFinished(Model.PlanetarySystem, out var winner))
            {
                Debug.LogError("Mission is not finished");
                return;
            }

            Model.ExecutionResult = new ExecutionResult(winner);
        }
    }
}