using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Tests.Setup.Abstractions;
using Kugushev.Scripts.Tests.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Setup
{
    public class MissionExecutionAndDebriefingTestingManager : BaseExecutionTestingManager
    {
        [SerializeField] private AchievementsManager achievementsManager;
        [SerializeField] private MissionSceneResultPipeline missionSceneResultPipeline;

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
    }
}