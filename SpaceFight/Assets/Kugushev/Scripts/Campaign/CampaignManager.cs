using System;
using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Game.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign
{
    internal class CampaignManager : BaseManager<CampaignModel>
    {
        [SerializeField] private CampaignModelProvider modelProvider;
        [SerializeField] private CampaignSceneParametersPipeline campaignSceneParametersPipeline;
        [SerializeField] private MissionSceneParametersPipeline missionSceneParametersPipeline;
        [SerializeField] private MissionSceneResultPipeline missionSceneResultPipeline;
        [SerializeField] private ExitState onMissionExitTransition;
        
        protected override CampaignModel InitRootModel()
        {
            var campaignInfo = campaignSceneParametersPipeline.Get();

            var model = new CampaignModel(campaignInfo);
            modelProvider.Set(this, model);

            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            CampaignModel rootModel)
        {
            var campaignProgressState = new CampaignProgressState(rootModel);
            var missionState = new MissionState(rootModel, missionSceneParametersPipeline, missionSceneResultPipeline);

            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, campaignProgressState)
                    }
                },
                {
                    campaignProgressState, new[]
                    {
                        new TransitionRecord(new ToMissionTransition(rootModel), missionState)
                    }
                },
                {
                    missionState, new []
                    {
                        new TransitionRecord(onMissionExitTransition, campaignProgressState)
                    }
                }
            };
        }

        protected override void OnStart()
        {
            Random.InitState(RootModel.CampaignInfo.Seed);
        }

        protected override void Dispose()
        {
            modelProvider.Cleanup(this);
        }
    }
}