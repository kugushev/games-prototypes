using System.Collections.Generic;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ProceduralGeneration;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.Utils;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign
{
    internal class CampaignManager : BaseManager<CampaignModel>
    {
        [SerializeField] private ObjectsPool? objectsPool;
        [SerializeField] private CampaignModelProvider? modelProvider;
        [SerializeField] private CampaignSceneParametersPipeline? campaignSceneParametersPipeline;
        [SerializeField] private CampaignSceneResultPipeline? campaignSceneResultPipeline;
        [SerializeField] private MissionsGenerator? missionsGenerationService;
        [SerializeField] private PoliticalActionsRepository? politicalActionsRepository;

        [Header("Mission Parameters")] [SerializeField]
        private MissionSceneParametersPipeline? missionSceneParametersPipeline;

        [SerializeField] private MissionSceneResultPipeline? missionSceneResultPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState? campaignExitState;

        [SerializeField] private TriggerTransition? toFinishTransition;

        [SerializeField] private ExitState? onMissionExitTransition;
        [SerializeField] private TriggerTransition? toMissionTransition;

        private readonly CampaignFinalizationState _finalizationState = new CampaignFinalizationState();

        protected override CampaignModel InitRootModel()
        {
            Asserting.NotNull(campaignSceneParametersPipeline, objectsPool, modelProvider);

            var campaignInfo = campaignSceneParametersPipeline.Get();
            var budget = campaignInfo.Budget ?? CampaignConstants.MaxBudget;

            var model = objectsPool.GetObject<CampaignModel, CampaignModel.State>(new CampaignModel.State(campaignInfo,
                objectsPool.GetObject<MissionSelection, MissionSelection.State>(new MissionSelection.State(budget)),
                objectsPool.GetObject<Playground, Playground.State>(default),
                objectsPool.GetObject<PlayerPerks, PlayerPerks.State>(
                    new PlayerPerks.State(campaignInfo.AvailablePerks)),
                objectsPool.GetObject<CampaignResult, CampaignResult.State>(default)
            ));

            modelProvider.Set(model);

            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            CampaignModel rootModel)
        {
            if (rootModel.CampaignInfo.IsPlayground)
            {
                return GetPlaygroundStates();
            }

            return GetCampaignStates();

            IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> GetPlaygroundStates()
            {
                Asserting.NotNull(politicalActionsRepository, missionSceneParametersPipeline, missionSceneResultPipeline,
                    toMissionTransition, toFinishTransition, onMissionExitTransition, campaignExitState);

                var playgroundState = new PlaygroundState(rootModel, politicalActionsRepository);
                var missionState = new MissionState(rootModel, missionSceneParametersPipeline,
                    missionSceneResultPipeline);

                _finalizationState.Setup(rootModel.CampaignResult, null);

                return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
                {
                    {
                        EntryState.Instance, new[]
                        {
                            new TransitionRecord(ImmediateTransition.Instance, playgroundState)
                        }
                    },
                    {
                        playgroundState, new[]
                        {
                            new TransitionRecord(toMissionTransition, missionState),
                            new TransitionRecord(toFinishTransition, _finalizationState)
                        }
                    },
                    {
                        missionState, new[]
                        {
                            new TransitionRecord(onMissionExitTransition, playgroundState)
                        }
                    },
                    {
                        _finalizationState, new[]
                        {
                            new TransitionRecord(ImmediateTransition.Instance, campaignExitState)
                        }
                    }
                };
            }

            IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> GetCampaignStates()
            {
                Asserting.NotNull(missionsGenerationService, missionSceneParametersPipeline, missionSceneResultPipeline,
                    toMissionTransition, toFinishTransition, onMissionExitTransition, campaignExitState);
                
                var missionSelectionState = new MissionSelectionState(RootModel, missionsGenerationService);

                var missionState = new MissionState(rootModel, missionSceneParametersPipeline,
                    missionSceneResultPipeline);

                _finalizationState.Setup(rootModel.CampaignResult,
                    rootModel.CampaignInfo.IsStandalone ? null : campaignSceneResultPipeline);

                return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
                {
                    {
                        EntryState.Instance, new[]
                        {
                            new TransitionRecord(ImmediateTransition.Instance, missionSelectionState)
                        }
                    },
                    {
                        missionSelectionState, new[]
                        {
                            new TransitionRecord(toMissionTransition, missionState),
                            new TransitionRecord(toFinishTransition, _finalizationState)
                        }
                    },
                    {
                        missionState, new[]
                        {
                            new TransitionRecord(onMissionExitTransition, missionSelectionState)
                        }
                    },
                    {
                        _finalizationState, new[]
                        {
                            new TransitionRecord(ImmediateTransition.Instance, campaignExitState)
                        }
                    }
                };
            }
        }

        protected override void OnStart()
        {
            Random.InitState(RootModel.CampaignInfo.Seed);
        }

        protected override void Dispose()
        {
            if (modelProvider is { }) 
                modelProvider.Cleanup();
        }
    }
}