using System;
using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Setup
{
    public class MissionBriefingTestingManager : BaseManager<MissionModel>
    {
        [SerializeField] protected ObjectsPool objectsPool;
        [SerializeField] private PlanetarySystemGenerator planetarySystemGenerator;
        [SerializeField] private MissionModelProvider missionModelProvider;

        public static int? Seed { get; set; }

        protected override MissionModel InitRootModel()
        {
            var seed = Seed ?? DateTime.Now.Millisecond;
            var missionProperties = new MissionProperties(seed);

            var planetarySystem = planetarySystemGenerator.CreatePlanetarySystem(missionProperties,
                Faction.Green, new PlanetarySystemProperties());

            var green = new ConflictParty(Faction.Green, default, default);
            var red = new ConflictParty(Faction.Red, default, default);

            var model = objectsPool.GetObject<MissionModel, MissionModel.State>(new MissionModel.State(
                new MissionInfo(missionProperties, new PlayerAchievements()),
                planetarySystem, green, red, Faction.Green
            ));

            missionModelProvider.Set(model);

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

        protected override void Dispose()
        {
        }
    }
}