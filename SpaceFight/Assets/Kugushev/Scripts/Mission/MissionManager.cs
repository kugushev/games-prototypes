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
        [Header("Boilerplate")] [SerializeField]
        private MissionModelProvider modelProvider;

        [SerializeField] private MissionSceneParametersPipeline missionSceneParametersPipeline;
        [SerializeField] private ExitState missionExitState;

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
            var debriefingState = new DebriefingState(rootModel);
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
                        new TransitionRecord(new ToExecutionTransition(rootModel), executionState)
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
                        new TransitionRecord(new ToExitMissionTransition(), missionExitState)
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