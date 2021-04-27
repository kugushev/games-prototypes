using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ProceduralGeneration;
using Kugushev.Scripts.Campaign.StatesAndTransitions;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Tests.Integration.Campaign.Setup.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Campaign.Setup
{
    internal class MissionSelectionTestingManager : BaseCampaignTestingManager
    {
        [SerializeField] private MissionsGenerator? missionsGenerationService;

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> ComposeStateMachine(
            CampaignModel rootModel)
        {
            Asserting.NotNull(missionsGenerationService);
            
            var missionSelectionState = new MissionSelectionState(rootModel, missionsGenerationService);

            return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecordOld(ImmediateTransition.Instance, missionSelectionState)
                    }
                }
            };
        }
    }
}