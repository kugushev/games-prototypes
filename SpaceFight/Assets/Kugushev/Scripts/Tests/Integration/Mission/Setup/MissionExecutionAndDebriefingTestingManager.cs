﻿using System.Collections.Generic;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Tests.Integration.Mission.Setup.Abstractions;
using Kugushev.Scripts.Tests.Integration.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Mission.Setup
{
    public class MissionExecutionAndDebriefingTestingManager : BaseMissionTestingManager
    {
        [Header(nameof(MissionExecutionAndDebriefingTestingManager))] [SerializeField]
        private PerksRegistry? achievementsManager;

        // [SerializeField] private MissionSceneResultPipeline? missionSceneResultPipeline;
        [SerializeField] private SuicideAI? suicideAI;
        [SerializeField] private SimpleAI? aggressiveAI;
        [SerializeField] private SimpleAI? normalAIGreen;
        [SerializeField] private SimpleAI? normalAIRed;

        public static bool GreenIsNormal { get; set; }
        public static bool RedIsNormal { get; set; }

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var executionState = new ExecutionState(rootModel, eventsCollector!);
            // var debriefingState = new DebriefingState(rootModel, missionSceneResultPipeline!, achievementsManager!,
            //     objectsPool!);

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
                },
                // {
                //     SingletonState.Instance, new[]
                //     {
                //         new TransitionRecordOld(ImmediateTransition.Instance, debriefingState)
                //     }
                // }
            };
        }

        protected override ICommander GreenCommander => GreenIsNormal ? normalAIGreen! : aggressiveAI!;
        protected override ICommander RedCommander => RedIsNormal ? (ICommander) normalAIRed! : suicideAI!;
    }
}