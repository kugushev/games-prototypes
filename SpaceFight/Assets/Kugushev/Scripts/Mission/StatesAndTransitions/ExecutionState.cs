using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class ExecutionState : BaseSceneLoadingState<MissionModel>
    {
        private readonly MissionEventsCollector _eventsCollector;

        public ExecutionState(MissionModel model, MissionEventsCollector eventsCollector)
            : base(model, UnityConstants.Scenes.MissionExecutionScene, true)
        {
            _eventsCollector = eventsCollector;
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

        protected override void OnEnterAfterLoadScene() => _eventsCollector.Start();

        protected override void OnExitBeforeUnloadScene()
        {
            if (!ToDebriefingTransition.IsMissionFinished(Model.PlanetarySystem, out var winner))
            {
                Debug.LogError("Mission is not finished");
                return;
            }

            Model.ExecutionResult = new ExecutionResult(winner);
            _eventsCollector.Stop();
        }
    }
}