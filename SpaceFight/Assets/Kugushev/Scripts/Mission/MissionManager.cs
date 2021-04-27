using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Player;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.Services;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission
{
    public class MissionManager : BaseManager<MissionModel>
    {
        [SerializeField] private ObjectsPool? objectsPool;
        [SerializeField] private MissionModelProvider? modelProvider;

        [Header("Parameters")] [SerializeField]
        private MissionSceneParametersPipeline? missionSceneParametersPipeline;

        [SerializeField] private MissionSceneResultPipeline? missionSceneResultPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState? missionExitState;

        [SerializeField] private TriggerTransition? toExecutionTransition;
        [SerializeField] private TriggerTransition? toFinishMissionTransition;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator? planetarySystemGenerator;

        [Header("Mission Related Assets")] [SerializeField]
        private Faction playerFaction = Faction.Green;

        [SerializeField] private PlayerPropertiesService? playerPropertiesService;
        [SerializeField] private MissionEventsCollector? eventsCollector;
        [SerializeField] private PerksManager? achievementsManager;

        [SerializeField] private PlayerCommander? playerCommander;
        [SerializeField] private SimpleAI? enemyAi;
        [SerializeField] private Fleet? greenFleet;
        [SerializeField] private Fleet? redFleet;


        protected override MissionModel InitRootModel()
        {
            Asserting.NotNull(missionSceneParametersPipeline, playerPropertiesService, greenFleet, redFleet,
                planetarySystemGenerator, playerCommander, enemyAi, objectsPool, modelProvider);

            var missionInfo = missionSceneParametersPipeline.Get();

            var (planetarySystemProperties, fleetProperties) =
                playerPropertiesService.GetPlayerProperties(playerFaction, missionInfo);

            switch (playerFaction)
            {
                case Faction.Green:
                    greenFleet.SetFleetProperties(fleetProperties);
                    break;
                case Faction.Red:
                    redFleet.SetFleetProperties(fleetProperties);
                    break;
            }

            var planetarySystem = planetarySystemGenerator.CreatePlanetarySystem(
                missionInfo.MissionInfo, playerFaction, planetarySystemProperties);

            var green = new ConflictParty(Faction.Green, greenFleet, playerCommander);
            var red = new ConflictParty(Faction.Red, redFleet, enemyAi);

            var model = objectsPool.GetObject<MissionModel, MissionModel.State>(
                new MissionModel.State(missionInfo, planetarySystem, green, red, playerFaction));
            modelProvider.Set(model);

            return model;
        }

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> ComposeStateMachine(
            MissionModel rootModel)
        {
            Asserting.NotNull(eventsCollector, missionSceneResultPipeline, achievementsManager, objectsPool,
                toExecutionTransition, toFinishMissionTransition, missionExitState);

            var briefingState = new BriefingState(rootModel);
            var executionState = new ExecutionState(rootModel, eventsCollector);
            var debriefingState = new DebriefingState(rootModel, missionSceneResultPipeline, achievementsManager,
                objectsPool);

            return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecordOld(ImmediateTransition.Instance, briefingState)
                    }
                },
                {
                    briefingState, new[]
                    {
                        new TransitionRecordOld(toExecutionTransition, executionState)
                    }
                },
                {
                    executionState, new[]
                    {
                        new TransitionRecordOld(new ToDebriefingTransition(rootModel), debriefingState)
                    }
                },
                {
                    debriefingState, new[]
                    {
                        new TransitionRecordOld(toFinishMissionTransition, missionExitState)
                    }
                }
            };
        }

        protected override void OnStart()
        {
            if (eventsCollector is { })
                eventsCollector.Cleanup();
        }

        protected override void Dispose()
        {
            if (eventsCollector is { })
                eventsCollector.Cleanup();
            if (modelProvider is { })
                modelProvider.Cleanup();
            if (greenFleet is { })
                greenFleet.ClearFleetProperties();
            if (redFleet is { })
                redFleet.ClearFleetProperties();
        }
    }
}