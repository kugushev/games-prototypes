﻿using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;
using Planet = Kugushev.Scripts.Mission.Models.Planet;

namespace Kugushev.Scripts.Tests.Unit.Utils
{
    public static class TestData
    {
        public const float ArmyPower = 50f;

        public static ArmyOld CreateArmy(FleetPerks? fleetPerks, Faction faction, float power = ArmyPower,
            Planet? targetPlanet = null)
        {
            const float magicNum = 42f;

            var objectsPool = ScriptableObject.CreateInstance<ObjectsPool>();
            var eventsCollector = ScriptableObject.CreateInstance<MissionEventsCollector>();

            if (fleetPerks == null)
                (_, fleetPerks) = PerksHelper.GetPlayerProperties();

            var sourcePlanet = objectsPool.GetObject<Planet, Planet.State>(default);
            var order = objectsPool.GetObject<Order, Order.State>(new Order.State(sourcePlanet, new Percentage(1f)));
            order.RegisterMovement(Vector3.zero);
            order.Commit(targetPlanet ?? objectsPool.GetObject<Planet, Planet.State>(default));

            var army = new ArmyOld(objectsPool);
            army.SetState(new ArmyOld.State(order, magicNum, magicNum, faction, power,
                new Sun(new Position(Vector3.zero), 0.5f),
                fleetPerks, eventsCollector));
            return army;
        }

        public static Planet CreatePlanet(float production, Faction faction,
            params (PerkId, int? level, PerkType)[] achievements) =>
            CreatePlanet(production, faction, 0f, achievements);

        public static Planet CreatePlanet(float production, Faction faction, float power,
            params (PerkId, int? level, PerkType)[] achievements)
        {
            var (planetarySystemProperties, _) = PerksHelper.GetPlayerProperties(achievements);

            var planet = new Planet(ScriptableObject.CreateInstance<ObjectsPool>());

            var eventsCollector = ScriptableObject.CreateInstance<MissionEventsCollector>();

            planet.SetState(new Planet.State(faction, default, production, default, default, eventsCollector,
                planetarySystemProperties, power));

            return planet;
        }
    }
}