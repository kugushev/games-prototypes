using System;
using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Common.FiniteStateMachine.StatesAndTransitions;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Game.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign
{
    internal class CampaignManager : BaseManager<CampaignModel>
    {
        [SerializeField] private CampaignModelProvider modelProvider;
        [SerializeField] private CampaignSceneParametersPipeline campaignSceneParametersPipeline;

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

            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, campaignProgressState)
                    }
                }
            };
        }
        
        protected override void OnStart()
        {
            if (!modelProvider.TryGetModel(out var model))
            {
                Debug.LogError("Unable to get model");
                return;
            }

            // let's init all required things here
            Random.InitState(model.CampaignInfo.Seed);
        }

        private void OnDestroy()
        {
            modelProvider.Cleanup(this);
        }
    }
}