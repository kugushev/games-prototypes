﻿using System.Collections.Generic;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Tests.Setup.Abstractions;
using Kugushev.Scripts.Tests.Utils;

namespace Kugushev.Scripts.Tests.Setup
{
    public class MissionExecutionTestingManager : BaseExecutionTestingManager
    {
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
    }
}