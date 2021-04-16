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
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.Green.Commander.AssignFleet(Model.Green.Fleet, Model.Green.Faction);
            Model.Red.Commander.AssignFleet(Model.Red.Fleet, Model.Red.Faction);
        }

        protected override void OnEnterAfterLoadScene() => _eventsCollector.Start();

        protected override void OnExitBeforeUnloadScene()
        {
            var model = Model;
            if (!ToDebriefingTransition.IsMissionFinished(model.PlanetarySystem, model.Green, model.Red, out var winner)
            )
            {
                Debug.LogError("Mission is not finished");
                return;
            }

            model.ExecutionResult = new ExecutionResult(winner);
            _eventsCollector.Stop();
        }
    }
}