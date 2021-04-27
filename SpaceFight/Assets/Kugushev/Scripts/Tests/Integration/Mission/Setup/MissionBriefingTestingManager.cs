using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.StatesAndTransitions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Mission.Setup
{
    public class MissionBriefingTestingManager : BaseManager<MissionModel>
    {
        [SerializeField] protected ObjectsPool? objectsPool;
        [SerializeField] private PlanetarySystemGenerator? planetarySystemGenerator;
        [SerializeField] private MissionModelProvider? missionModelProvider;

        public static int? Seed { get; set; }

        protected override MissionModel InitRootModel()
        {
            var seed = Seed ?? DateTime.Now.Millisecond;
            var missionProperties = new MissionInfo(seed, Difficulty.Normal,
                ScriptableObject.CreateInstance<Intrigue>());

            var planetarySystemPerks = objectsPool!.GetObject<PlanetarySystemPerks, PlanetarySystemPerks.State>(
                new PlanetarySystemPerks.State());
            var planetarySystem = planetarySystemGenerator!.CreatePlanetarySystem(missionProperties, Faction.Green,
                planetarySystemPerks);

            var green = new ConflictParty(Faction.Green, default!, default!);
            var red = new ConflictParty(Faction.Red, default!, default!);

            var model = objectsPool.GetObject<MissionModel, MissionModel.State>(new MissionModel.State(
                new MissionParameters(missionProperties, objectsPool.GetObject<PlayerPerks, PlayerPerks.State>(
                    new PlayerPerks.State(PerkIdHelper.AllPerks))),
                planetarySystem, green, red, Faction.Green
            ));

            missionModelProvider!.Set(model);

            return model;
        }

        protected override IReadOnlyDictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>> ComposeStateMachine(
            MissionModel rootModel)
        {
            var briefingState = new BriefingState(rootModel);
            return new Dictionary<IUnparameterizedState, IReadOnlyList<TransitionRecordOld>>
            {
                {
                    EntryState.Instance, new[]
                    {
                        new TransitionRecordOld(ImmediateTransition.Instance, briefingState)
                    }
                }
            };
        }

        protected override void Dispose()
        {
        }
    }
}