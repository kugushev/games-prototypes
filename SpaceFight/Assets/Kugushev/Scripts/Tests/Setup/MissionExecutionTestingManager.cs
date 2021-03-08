using System.Collections.Generic;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using Kugushev.Scripts.Tests.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Setup
{
    public class MissionExecutionTestingManager : BaseManager<MissionModel>
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private MissionModelProvider modelProvider;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator planetarySystemGenerator;

        [Header("Mission Related Assets")] [SerializeField]
        private MissionEventsCollector eventsCollector;

        [SerializeField] private SimpleAI greenAi;
        [SerializeField] private SimpleAI redAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        public static int Seed { get; set; }

        protected override MissionModel InitRootModel()
        {
            var missionInfo = new MissionInfo(Seed);
            var planetarySystem = planetarySystemGenerator.CreatePlanetarySystem(missionInfo.Seed);
            var green = new ConflictParty(Faction.Green, greenFleet, greenAi);
            var red = new ConflictParty(Faction.Red, redFleet, redAi);

            var model = objectsPool.GetObject<MissionModel, MissionModel.State>(
                new MissionModel.State(missionInfo, planetarySystem, green, red));
            
            modelProvider.Set(model);
            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var executionState = new ExecutionState(rootModel);
            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, executionState)
                    }
                },
                {
                    executionState, new[]
                    {
                        new TransitionRecord(new ToDebriefingTransition(rootModel), SingletonState.Instance)
                    }
                }
            };
        }

        protected override void OnStart()
        {
            eventsCollector.Cleanup();
        }

        protected override void Dispose()
        {
            eventsCollector.Cleanup();
            modelProvider.Cleanup();
            RootModel.Dispose();
        }
    }
}