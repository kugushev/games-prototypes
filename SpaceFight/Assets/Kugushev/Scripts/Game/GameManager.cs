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

        [Header("States and Transitions")] [SerializeField]
        private ExitState onCampaignExitTransition;

        [SerializeField] private TriggerTransition toCampaignTransition;
        [SerializeField] private TriggerTransition toPlaygroundTransition;


        protected override GameModel InitRootModel()
        {
            var model = new GameModel();
            gameModelProvider.Set(model);
            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            GameModel rootModel)
        {
            var mainMenuState = new MainMenuState(rootModel);
            var campaignState = new CampaignState(rootModel, campaignSceneParametersPipeline, false);
            var playgroundState = new CampaignState(rootModel, campaignSceneParametersPipeline, true);

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
                        new TransitionRecord(toCampaignTransition, campaignState),
                        new TransitionRecord(toPlaygroundTransition, playgroundState)
                    }
                },
                {
                    campaignState, new[]
                    {
                        new TransitionRecord(onCampaignExitTransition, mainMenuState)
                    }
                },
                {
                    playgroundState, new[]
                    {
                        new TransitionRecord(onCampaignExitTransition, mainMenuState)
                    }
                }
            };
        }

        protected override void Dispose()
        {
            gameModelProvider.Cleanup();
        }
    }
}