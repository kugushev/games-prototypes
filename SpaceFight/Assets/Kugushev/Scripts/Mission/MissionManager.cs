using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Player;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission
{
    public class MissionManager : BaseManager<MissionModel>
    {
        [Header("Boilerplate")] [SerializeField]
        private MissionModelProvider modelProvider;

        [SerializeField] private MissionSceneParametersPipeline missionSceneParametersPipeline;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator planetarySystemGenerator;

        [Header("Mission Related Assets")] [SerializeField]
        private PlayerCommander playerCommander;

        [SerializeField] private SimpleAI enemyAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        protected override MissionModel InitRootModel()
        {
            var missionInfo = missionSceneParametersPipeline.Get();

            var model = new MissionModel(missionInfo);
            modelProvider.Set(this, model);

            return model;
        }

        protected override IReadOnlyDictionary<IState, IReadOnlyList<TransitionRecord>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var briefingState = new BriefingState(rootModel);
            return new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecord(ImmediateTransition.Instance, briefingState)
                    }
                }
            };
        }

        protected override void OnStart()
        {
            RootModel.PlanetarySystem = planetarySystemGenerator.CreatePlanetarySystem(RootModel.Info.Seed);
            RootModel.Green = new ConflictParty(Faction.Green, greenFleet, playerCommander);
            RootModel.Red = new ConflictParty(Faction.Red, redFleet, enemyAi);
        }

        private void OnDestroy()
        {
            modelProvider.Cleanup(this);
            RootModel.Dispose();
        }
    }
}