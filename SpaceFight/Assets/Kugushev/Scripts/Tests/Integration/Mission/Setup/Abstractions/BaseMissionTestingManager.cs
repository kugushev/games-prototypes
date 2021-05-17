using Kugushev.Scripts.Common.Manager;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.Services;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Integration.Mission.Setup.Abstractions
{
    public abstract class BaseMissionTestingManager : BaseManager<MissionModel>
    {
        [SerializeField] protected ObjectsPool? objectsPool;
        [SerializeField] private MissionModelProvider? modelProvider;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator? planetarySystemGenerator;

        [Header("Mission Related Assets")] [SerializeField]
        private Faction playerFaction = Faction.Green;

        [SerializeField] private PlayerPropertiesServiceOld? playerPropertiesService;
        [SerializeField] protected MissionEventsCollector? eventsCollector;

        [SerializeField] private Fleet? greenFleet;
        [SerializeField] private Fleet? redFleet;

        // public static MissionParameters? MissionInfo { get; set; }
        public static MissionModel? MissionModel { get; private set; }

        protected override MissionModel InitRootModel()
        {
            return null;
            // if (MissionInfo == null)
            //     throw new Exception("Mission info is null");
            // var missionInfo = MissionInfo.Value;
            // MissionInfo = null;

            // var (planetarySystemProperties, fleetProperties) =
            //     playerPropertiesService!.GetPlayerProperties(playerFaction, missionInfo);
            //
            // switch (playerFaction)
            // {
            //     case Faction.Green:
            //         greenFleet!.SetFleetProperties(fleetProperties);
            //         break;
            //     case Faction.Red:
            //         redFleet!.SetFleetProperties(fleetProperties);
            //         break;
            // }
            //
            // var planetarySystem = planetarySystemGenerator!.CreatePlanetarySystem(missionInfo.MissionInfo,
            //     Faction.Green, planetarySystemProperties);
            // var green = new ConflictParty(Faction.Green, greenFleet!, GreenCommander);
            // var red = new ConflictParty(Faction.Red, redFleet!, RedCommander);
            //
            // var model = objectsPool!.GetObject<MissionModel, MissionModel.State>(
            //     new MissionModel.State(missionInfo, planetarySystem, green, red, playerFaction));
            //
            // modelProvider!.Set(model);
            //
            // MissionModel = model;
            //
            // return model;
        }

        protected abstract ICommander GreenCommander { get; }
        protected abstract ICommander RedCommander { get; }

        protected override void OnStart()
        {
            eventsCollector!.Cleanup();
        }

        protected override void Dispose()
        {
            eventsCollector!.Cleanup();
            modelProvider!.Cleanup();
            RootModel.Dispose();
        }
    }
}