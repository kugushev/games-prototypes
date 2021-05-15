using System.Collections.Generic;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.Repositories;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign
{
    internal class CampaignManager : BaseManager<CampaignModelOld>
    {
        [SerializeField] private ObjectsPool? objectsPool;

        [SerializeField] private CampaignModelProvider? modelProvider;

        //[SerializeField] private CampaignSceneParametersPipeline? campaignSceneParametersPipeline;
        // [SerializeField] private CampaignSceneResultPipeline? campaignSceneResultPipeline;
        // [SerializeField] private MissionsGenerator? missionsGenerationService;
        [SerializeField] private IntriguesRepository? politicalActionsRepository;

        [Header("Mission Parameters")] [SerializeField]
        private MissionSceneParametersPipeline? missionSceneParametersPipeline;

        [SerializeField] private MissionSceneResultPipeline? missionSceneResultPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState? campaignExitState;

        [SerializeField] private TriggerTransition? toFinishTransition;

        [SerializeField] private ExitState? onMissionExitTransition;
        [SerializeField] private TriggerTransition? toMissionTransition;

        private readonly CampaignFinalizationState _finalizationState = new CampaignFinalizationState();

        protected override CampaignModelOld InitRootModel()
        {
            Asserting.NotNull(objectsPool, modelProvider);

            var campaignInfo = new CampaignParameters();
            var budget = campaignInfo.Budget;

            var model = objectsPool.GetObject<CampaignModelOld, CampaignModelOld.State>(new CampaignModelOld.State(
                campaignInfo,
                objectsPool.GetObject<MissionSelection, MissionSelection.State>(new MissionSelection.State(budget)),
                objectsPool.GetObject<Playground, Playground.State>(default),
                objectsPool.GetObject<PlayerPerksOld, PlayerPerksOld.State>(
                    new PlayerPerksOld.State(campaignInfo.AvailablePerks)),
                objectsPool.GetObject<CampaignResult, CampaignResult.State>(default)
            ));

            modelProvider.Set(model);

            return model;
        }

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            ComposeStateMachine(
                CampaignModelOld rootModelOld)
        {
            if (rootModelOld.CampaignParameters.IsPlayground)
            {
                return GetPlaygroundStates();
            }

            return GetCampaignStates();

            IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> GetPlaygroundStates()
            {
                Asserting.NotNull(politicalActionsRepository, missionSceneParametersPipeline,
                    missionSceneResultPipeline,
                    toMissionTransition, toFinishTransition, onMissionExitTransition, campaignExitState);

                var playgroundState = new PlaygroundState(rootModelOld, politicalActionsRepository);
                var missionState = new MissionState(rootModelOld, missionSceneParametersPipeline,
                    missionSceneResultPipeline);

                _finalizationState.Setup(rootModelOld.CampaignResult);

                return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
                {
                    {
                        EntryState.Instance, new[]
                        {
                            new TransitionRecordOld(ImmediateTransition.Instance, playgroundState)
                        }
                    },
                    {
                        playgroundState, new[]
                        {
                            new TransitionRecordOld(toMissionTransition, missionState),
                            new TransitionRecordOld(toFinishTransition, _finalizationState)
                        }
                    },
                    {
                        missionState, new[]
                        {
                            new TransitionRecordOld(onMissionExitTransition, playgroundState)
                        }
                    },
                    {
                        _finalizationState, new[]
                        {
                            new TransitionRecordOld(ImmediateTransition.Instance, campaignExitState)
                        }
                    }
                };
            }

            IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> GetCampaignStates()
            {
                // Asserting.NotNull(missionsGenerationService, missionSceneParametersPipeline, missionSceneResultPipeline,
                //     toMissionTransition, toFinishTransition, onMissionExitTransition, campaignExitState);

                var missionSelectionState = new MissionSelectionState(RootModel, null);

                var missionState = new MissionState(rootModelOld, missionSceneParametersPipeline,
                    missionSceneResultPipeline);

                _finalizationState.Setup(rootModelOld.CampaignResult);
                //rootModel.CampaignParameters.IsStandalone ? null : campaignSceneResultPipeline);

                return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
                {
                    {
                        EntryState.Instance, new[]
                        {
                            new TransitionRecordOld(ImmediateTransition.Instance, missionSelectionState)
                        }
                    },
                    {
                        missionSelectionState, new[]
                        {
                            new TransitionRecordOld(toMissionTransition, missionState),
                            new TransitionRecordOld(toFinishTransition, _finalizationState)
                        }
                    },
                    {
                        missionState, new[]
                        {
                            new TransitionRecordOld(onMissionExitTransition, missionSelectionState)
                        }
                    },
                    {
                        _finalizationState, new[]
                        {
                            new TransitionRecordOld(ImmediateTransition.Instance, campaignExitState)
                        }
                    }
                };
            }
        }

        protected override void OnStart()
        {
            Random.InitState(RootModel.CampaignParameters.Seed);
        }

        protected override void Dispose()
        {
            if (modelProvider is { })
                modelProvider.Cleanup();
        }
    }
}