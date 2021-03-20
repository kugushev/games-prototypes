using System.Collections.Generic;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Tests.Integration.Mission.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Mission.Setup
{
    public class MissionMissionTestingManager : BaseMissionTestingManager
    {
        [Header(nameof(MissionMissionTestingManager))] [SerializeField]
        private SimpleAI greenAi;

        [SerializeField] private SimpleAI redAi;

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var executionState = new ExecutionState(rootModel);
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
                }
            };
        }


        protected override ICommander GreenCommander => greenAi;
        protected override ICommander RedCommander => redAi;
    }
}