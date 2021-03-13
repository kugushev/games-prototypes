using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Tests.Integration.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Setup
{
    public class MissionExecutionAndDebriefingTestingManager : BaseExecutionTestingManager
    {
        [Header(nameof(MissionExecutionAndDebriefingTestingManager))] [SerializeField]
        private AchievementsManager achievementsManager;

        [SerializeField] private MissionSceneResultPipeline missionSceneResultPipeline;
        [SerializeField] private SuicideAI suicideAI;
        [SerializeField] private SimpleAI aggressiveAI;

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var executionState = new ExecutionState(rootModel);
            var debriefingState = new DebriefingState(rootModel, missionSceneResultPipeline, achievementsManager,
                objectsPool);

            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, executionState)
                    }
                },
                {
                    executionState, new[]
                    {
                        new TransitionRecord(new ToDebriefingTransition(rootModel), SingletonState.Instance)
                    }
                },
                {
                    SingletonState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, debriefingState)
                    }
                }
            };
        }

        protected override ICommander GreenCommander => aggressiveAI;
        protected override ICommander RedCommander => suicideAI;
    }
}