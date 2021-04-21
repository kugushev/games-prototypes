using System.Collections.Generic;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ProceduralGeneration;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.StatesAndTransitions;
using Kugushev.Scripts.Game.Utils;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameManager : BaseManager<GameModel>
    {
        [SerializeField] private ObjectsPool? objectsPool;
        [SerializeField] private GameModelProvider? gameModelProvider;
        [SerializeField] private GameSceneParametersPipeline? gameSceneParametersPipeline;

        [SerializeField] private ParliamentGenerator? parliamentGenerator;


        // ReSharper disable once NotAccessedField.Local
        [SerializeField] private PoliticalActionsRepository? tempPoliticalActionsRepository;

        [Header("States and Transitions")] [SerializeField]
        private ExitState? gameExitState;

        [SerializeField] private ExitState? onCampaignExitTransition;
        [SerializeField] private TriggerTransition? toCampaignTransition;
        [SerializeField] private TriggerTransition? toRevolutionTransition;
        [SerializeField] private TriggerTransition? toMainMenuTransition;


        [Header("Campaign")] [SerializeField] private CampaignSceneParametersPipeline? campaignSceneParametersPipeline;
        [SerializeField] private CampaignSceneResultPipeline? campaignSceneResultPipeline;

        [Inject] private GameModeParameters _gameModeParameters;
        
        protected override GameModel InitRootModel()
        {
            Asserting.NotNull(gameSceneParametersPipeline, parliamentGenerator, objectsPool, gameModelProvider);

            var info = _gameModeParameters;// gameSceneParametersPipeline.Get();

            var parliament = parliamentGenerator.Generate(info.Seed);
            var campaignPreparation = objectsPool.GetObject<CampaignPreparation, CampaignPreparation.State>(
                new CampaignPreparation.State(parliament));
            var model = objectsPool.GetObject<GameModel, GameModel.State>(
                new GameModel.State(parliament, campaignPreparation));

            gameModelProvider.Set(model);
            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            GameModel rootModel)
        {
            Asserting.NotNull(campaignSceneParametersPipeline, campaignSceneResultPipeline, toCampaignTransition,
                toRevolutionTransition, onCampaignExitTransition, toMainMenuTransition, gameExitState);

            var politicsState = new PoliticsState(rootModel);
            var campaignState = new CampaignState(rootModel, campaignSceneParametersPipeline,
                campaignSceneResultPipeline);
            var revolutionState = new RevolutionState(rootModel);

            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, politicsState)
                    }
                },
                {
                    politicsState, new[]
                    {
                        new TransitionRecord(toCampaignTransition, campaignState),
                        new TransitionRecord(toRevolutionTransition, revolutionState)
                    }
                },
                {
                    campaignState, new[]
                    {
                        new TransitionRecord(onCampaignExitTransition, politicsState)
                    }
                },
                {
                    revolutionState, new[]
                    {
                        new TransitionRecord(toMainMenuTransition, gameExitState)
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