using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ProceduralGeneration;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Tests.Integration.Campaign.Setup.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Campaign.Setup
{
    internal class MissionSelectionTestingManager : BaseCampaignTestingManager
    {
        [SerializeField] private MissionsGenerator missionsGenerationService;

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            CampaignModel rootModel)
        {
            var missionSelectionState = new MissionSelectionState(rootModel, missionsGenerationService);

            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, missionSelectionState)
                    }
                }
            };
        }
    }
}