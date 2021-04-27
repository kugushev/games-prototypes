using System.Collections.Generic;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.App.StatesAndTransitions;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.App
{
    public class AppManager : BaseManager<AppModel>
    {
        [SerializeField] private AppModelProvider? gameModelProvider;
        [SerializeField] private CampaignSceneParametersPipeline? campaignSceneParametersPipeline;
        [SerializeField] private GameSceneParametersPipeline? gameSceneParametersPipeline;

        [Header("States and Transitions")] [SerializeField]
        private ExitState? onCampaignExitTransition;

        [SerializeField] private ExitState? onGameExitTransition;

        [SerializeField] private TriggerTransition? toCampaignTransition;
        [SerializeField] private TriggerTransition? toPlaygroundTransition;

        private ToNewGameTransition _toNewGameTransition = default!;

        protected override AppModel InitRootModel()
        {
            var sp = GetComponent<SpriteRenderer>();

            print(sp);

            var model = new AppModel();
            if (gameModelProvider is { })
                gameModelProvider.Set(model);

            return model;
        }

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> ComposeStateMachine(
            AppModel rootModel)
        {
            Asserting.NotNull(campaignSceneParametersPipeline, gameSceneParametersPipeline, toCampaignTransition,
                toPlaygroundTransition, onCampaignExitTransition, onGameExitTransition);

            var mainMenuState = new MainMenuState(rootModel);
            var campaignState = new CustomCampaignState(rootModel, campaignSceneParametersPipeline, false);
            var playgroundState = new CustomCampaignState(rootModel, campaignSceneParametersPipeline, true);
            var newGameState = new GameState(rootModel, gameSceneParametersPipeline);

            return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecordOld(ImmediateTransition.Instance, mainMenuState)
                    }
                },
                {
                    mainMenuState, new[]
                    {
                        new TransitionRecordOld(toCampaignTransition, campaignState),
                        new TransitionRecordOld(toPlaygroundTransition, playgroundState),
                        new TransitionRecordOld(_toNewGameTransition, newGameState)
                    }
                },
                {
                    campaignState, new[]
                    {
                        new TransitionRecordOld(onCampaignExitTransition, mainMenuState)
                    }
                },
                {
                    playgroundState, new[]
                    {
                        new TransitionRecordOld(onCampaignExitTransition, mainMenuState)
                    }
                },
                {
                    newGameState, new[]
                    {
                        new TransitionRecordOld(onGameExitTransition, mainMenuState)
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