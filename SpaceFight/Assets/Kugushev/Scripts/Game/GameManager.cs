using System.Collections.Generic;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ProceduralGeneration;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.StatesAndTransitions;
using Kugushev.Scripts.Game.Utils;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game
{
    public class GameManager : BaseManager<GameModel>
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private GameModelProvider gameModelProvider;
        [SerializeField] private GameSceneParametersPipeline gameSceneParametersPipeline;

        [SerializeField] private ParliamentGenerator parliamentGenerator;

        [SerializeField] private PoliticalActionsRepository tempPoliticalActionsRepository;

        [Header("States and Transitions")] [SerializeField]
        private ExitState onCampaignExitTransition;

        [SerializeField] private TriggerTransition toCampaignTransition;

        protected override GameModel InitRootModel()
        {
            var info = gameSceneParametersPipeline.Get();

            var parliament = parliamentGenerator.Generate(info.Seed);
            var model = objectsPool.GetObject<GameModel, GameModel.State>(new GameModel.State(parliament));

            var action = new List<PoliticalAction>();
            for (int i = 0; i < 7; i++)
                action.Add(tempPoliticalActionsRepository.GetRandom(Difficulty.Normal));
            for (int i = 0; i < 3; i++)
                action.Add(tempPoliticalActionsRepository.GetRandom(Difficulty.Hard));
            for (int i = 0; i < 2; i++)
                action.Add(tempPoliticalActionsRepository.GetRandom(Difficulty.Insane));
            model.AddPoliticalActions(action);


            gameModelProvider.Set(model);
            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            GameModel rootModel)
        {
            var politicsState = new PoliticsState(rootModel);
            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, politicsState)
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