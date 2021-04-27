using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.Modes;
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

        [Inject] private ParametersPipeline<GameModeParameters> _parametersPipeline = default!;


        protected override GameModel InitRootModel()
        {
            Asserting.NotNull(gameSceneParametersPipeline, parliamentGenerator, objectsPool, gameModelProvider);

            var info = _parametersPipeline.Pop();

            var parliament = parliamentGenerator.Generate(info.Seed);
            var campaignPreparation = objectsPool.GetObject<CampaignPreparation, CampaignPreparation.State>(
                new CampaignPreparation.State(parliament));
            var model = objectsPool.GetObject<GameModel, GameModel.State>(
                new GameModel.State(parliament, campaignPreparation));

            gameModelProvider.Set(model);
            return model;
        }

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            ComposeStateMachine(
                GameModel rootModel)
        {
            Asserting.NotNull(campaignSceneParametersPipeline, campaignSceneResultPipeline, toCampaignTransition,
                toRevolutionTransition, onCampaignExitTransition, toMainMenuTransition, gameExitState);

            var politicsState = new PoliticsState(rootModel);
            var campaignState = new CampaignState(rootModel, campaignSceneParametersPipeline,
                campaignSceneResultPipeline);
            var revolutionState = new RevolutionState(rootModel);

            return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecordOld(ImmediateTransition.Instance, politicsState)
                    }
                },
                {
                    politicsState, new[]
                    {
                        new TransitionRecordOld(toCampaignTransition, campaignState),
                        new TransitionRecordOld(toRevolutionTransition, revolutionState)
                    }
                },
                {
                    campaignState, new[]
                    {
                        new TransitionRecordOld(onCampaignExitTransition, politicsState)
                    }
                },
                {
                    revolutionState, new[]
                    {
                        new TransitionRecordOld(toMainMenuTransition, gameExitState)
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