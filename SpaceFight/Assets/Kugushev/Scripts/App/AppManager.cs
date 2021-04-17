using System.Collections.Generic;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.App.StatesAndTransitions;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.App
{
    internal class AppManager : BaseManager<AppModel>
    {
        [SerializeField] private AppModelProvider? gameModelProvider;
        [SerializeField] private CampaignSceneParametersPipeline? campaignSceneParametersPipeline;
        [SerializeField] private GameSceneParametersPipeline? gameSceneParametersPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState? onCampaignExitTransition;

        [SerializeField] private ExitState? onGameExitTransition;

        [SerializeField] private TriggerTransition? toCampaignTransition;
        [SerializeField] private TriggerTransition? toPlaygroundTransition;

        [SerializeField] private TriggerTransition? toNewGameTransition;

        protected override AppModel InitRootModel()
        {
            var model = new AppModel();
            if (gameModelProvider is { })
                gameModelProvider.Set(model);

            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            AppModel rootModel)
        {
            Asserting.NotNull(campaignSceneParametersPipeline, gameSceneParametersPipeline, toCampaignTransition,
                toPlaygroundTransition, toNewGameTransition, onCampaignExitTransition, onGameExitTransition);

            var mainMenuState = new MainMenuState(rootModel);
            var campaignState = new CustomCampaignState(rootModel, campaignSceneParametersPipeline, false);
            var playgroundState = new CustomCampaignState(rootModel, campaignSceneParametersPipeline, true);
            var newGameState = new GameState(rootModel, gameSceneParametersPipeline);

            // SignalListenerTransition<NewGameSignal> toNewGameTransition = new();
            
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
                        new TransitionRecord(toPlaygroundTransition, playgroundState),
                        new TransitionRecord(toNewGameTransition, newGameState)
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
                },
                {
                    newGameState, new[]
                    {
                        new TransitionRecord(onGameExitTransition, mainMenuState)
                    }
                }
            };
        }

        protected override void Dispose()
        {
            if (gameModelProvider is { })
                gameModelProvider.Cleanup();
        }
    }
}