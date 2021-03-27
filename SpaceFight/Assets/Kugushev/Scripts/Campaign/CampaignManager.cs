using System.Collections.Generic;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Services;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign
{
    internal class CampaignManager : BaseManager<CampaignModel>
    {
        [SerializeField] private CampaignModelProvider modelProvider;
        [SerializeField] private CampaignSceneParametersPipeline campaignSceneParametersPipeline;
        [SerializeField] private MissionsGenerationService missionsGenerationService;

        [Header("Mission Parameters")] [SerializeField]
        private MissionSceneParametersPipeline missionSceneParametersPipeline;

        [SerializeField] private MissionSceneResultPipeline missionSceneResultPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState campaignExitState;

        [SerializeField] private TriggerTransition toFinishTransition;

        [SerializeField] private ExitState onMissionExitTransition;
        [SerializeField] private TriggerTransition toMissionTransition;

        protected override CampaignModel InitRootModel()
        {
            var campaignInfo = campaignSceneParametersPipeline.Get();

            var model = new CampaignModel(campaignInfo);
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
                var playgroundState = new PlaygroundState(rootModel);
                var missionState = new MissionState(rootModel, missionSceneParametersPipeline,
                    missionSceneResultPipeline);

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
                            new TransitionRecord(toFinishTransition, campaignExitState)
                        }
                    },
                    {
                        missionState, new[]
                        {
                            new TransitionRecord(onMissionExitTransition, playgroundState)
                        }
                    }
                };
            }

            IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> GetCampaignStates()
            {
                var missionSelectionState = new MissionSelectionState(RootModel, missionsGenerationService);

                var missionState = new MissionState(rootModel, missionSceneParametersPipeline,
                    missionSceneResultPipeline);

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
                            new TransitionRecord(toFinishTransition, campaignExitState)
                        }
                    },
                    {
                        missionState, new[]
                        {
                            new TransitionRecord(onMissionExitTransition, missionSelectionState)
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
            modelProvider.Cleanup();
        }
    }
}