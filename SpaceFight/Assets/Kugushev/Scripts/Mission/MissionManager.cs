using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Player;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission
{
    public class MissionManager : BaseManager<MissionModel>
    {
        [SerializeField] private MissionModelProvider modelProvider;

        [Header("Parameters")] [SerializeField]
        private MissionSceneParametersPipeline missionSceneParametersPipeline;

        [SerializeField] private MissionSceneResultPipeline missionSceneResultPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState missionExitState;

        [SerializeField] private TriggerTransition toExecutionTransition;
        [SerializeField] private TriggerTransition toFinishMissionTransition;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator planetarySystemGenerator;

        [Header("Mission Related Assets")] [SerializeField]
        private PlayerCommander playerCommander;

        [SerializeField] private SimpleAI enemyAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        [SerializeField] private MissionEventsCollector eventsCollector;

        protected override MissionModel InitRootModel()
        {
            var missionInfo = missionSceneParametersPipeline.Get();

            var model = new MissionModel(missionInfo);
            modelProvider.Set(this, model);

            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var briefingState = new BriefingState(rootModel);
            var executionState = new ExecutionState(rootModel);
            var debriefingState = new DebriefingState(rootModel, missionSceneResultPipeline);
            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, briefingState)
                    }
                },
                {
                    briefingState, new[]
                    {
                        new TransitionRecord(toExecutionTransition, executionState)
                    }
                },
                {
                    executionState, new[]
                    {
                        new TransitionRecord(new ToDebriefingTransition(rootModel), debriefingState)
                    }
                },
                {
                    debriefingState, new[]
                    {
                        new TransitionRecord(toFinishMissionTransition, missionExitState)
                    }
                }
            };
        }

        protected override void OnStart()
        {
            eventsCollector.Cleanup();
            RootModel.PlanetarySystem = planetarySystemGenerator.CreatePlanetarySystem(RootModel.Info.Seed);
            RootModel.Green = new ConflictParty(Faction.Green, greenFleet, playerCommander);
            RootModel.Red = new ConflictParty(Faction.Red, redFleet, enemyAi);
        }

        protected override void Dispose()
        {
            eventsCollector.Cleanup();
            modelProvider.Cleanup(this);
            RootModel.Dispose();
        }
    }
}