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
    public class MissionExecutionTestingManager : BaseMissionTestingManager
    {
        [Header(nameof(MissionExecutionTestingManager))] [SerializeField]
        private SimpleAI? greenAi;

        [SerializeField] private SimpleAI? redAi;

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var executionState = new ExecutionState(rootModel, eventsCollector!);
            return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecordOld(ImmediateTransition.Instance, executionState)
                    }
                },
                {
                    executionState, new[]
                    {
                        new TransitionRecordOld(new ToDebriefingTransition(rootModel), SingletonState.Instance)
                    }
                }
            };
        }


        protected override ICommander GreenCommander => greenAi!;
        protected override ICommander RedCommander => redAi!;
    }
}