using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Setup.Abstractions
{
    public abstract class BaseExecutionTestingManager : BaseManager<MissionModel>
    {
        [SerializeField] protected ObjectsPool objectsPool;
        [SerializeField] private MissionModelProvider modelProvider;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator planetarySystemGenerator;

        [Header("Mission Related Assets")] [SerializeField]
        private MissionEventsCollector eventsCollector;

        [SerializeField] private SimpleAI greenAi;
        [SerializeField] private SimpleAI redAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        public static int Seed { get; set; }
        public static MissionModel MissionModel { get; private set; }

        protected override MissionModel InitRootModel()
        {
            var missionInfo = new MissionInfo(Seed, new PlayerAchievements());
            var planetarySystem = planetarySystemGenerator.CreatePlanetarySystem(missionInfo.Seed);
            var green = new ConflictParty(Faction.Green, greenFleet, greenAi);
            var red = new ConflictParty(Faction.Red, redFleet, redAi);

            var model = objectsPool.GetObject<MissionModel, MissionModel.State>(
                new MissionModel.State(missionInfo, planetarySystem, green, red, Faction.Green));

            modelProvider.Set(model);

            MissionModel = model;

            return model;
        }

        protected override void OnStart()
        {
            eventsCollector.Cleanup();
        }

        protected override void Dispose()
        {
            eventsCollector.Cleanup();
            modelProvider.Cleanup();
            RootModel.Dispose();
        }
    }
}