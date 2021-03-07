using System.Collections.Generic;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.StatesAndTransitions;
using Kugushev.Scripts.Game.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Game
{
    internal class GameManager : BaseManager<GameModel>
    {
        [SerializeField] private GameModelProvider gameModelProvider;
        [SerializeField] private CampaignSceneParametersPipeline campaignSceneParametersPipeline;
        [SerializeField] private TriggerTransition toCampaignTransition;

        protected override GameModel InitRootModel()
        {
            var model = new GameModel();
            gameModelProvider.Set(this, model);
            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            GameModel rootModel)
        {
            var mainMenuState = new MainMenuState(rootModel);
            var campaignState = new CampaignState(rootModel, campaignSceneParametersPipeline);

            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, mainMenuState)
                    }
                },
                {
                    mainMenuState, new[]
                    {
                        new TransitionRecord(toCampaignTransition, campaignState)
                    }
                }
            };
        }

        protected override void Dispose()
        {
            gameModelProvider.Cleanup(this);
        }
    }
}