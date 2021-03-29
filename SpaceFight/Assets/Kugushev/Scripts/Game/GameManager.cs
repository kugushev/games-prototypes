using System.Collections.Generic;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ProceduralGeneration;
using Kugushev.Scripts.Game.StatesAndTransitions;
using Kugushev.Scripts.Game.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Game
{
    public class GameManager : BaseManager<GameModel>
    {
        [SerializeField] private GameModelProvider gameModelProvider;
        [SerializeField] private GameSceneParametersPipeline gameSceneParametersPipeline;

        [SerializeField] private ParliamentGenerator parliamentGenerator;

        [Header("States and Transitions")] [SerializeField]
        private ExitState onCampaignExitTransition;

        [SerializeField] private TriggerTransition toCampaignTransition;

        protected override GameModel InitRootModel()
        {
            var info = gameSceneParametersPipeline.Get();

            var model = new GameModel(parliamentGenerator.Generate(info.Seed));
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