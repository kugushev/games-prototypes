using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.PlayerProperties;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Unit.Utils
{
    public static class Factory
    {
        public const float ArmyPower = 50f;

        public static Army CreateArmy(FleetProperties fleetProperties, Faction faction, float power = ArmyPower,
            Planet targetPlanet = null)
        {
            const float magicNum = 42f;

            var objectsPool = ScriptableObject.CreateInstance<ObjectsPool>();
            var eventsCollector = ScriptableObject.CreateInstance<MissionEventsCollector>();

            var sourcePlanet = objectsPool.GetObject<Planet, Planet.State>(default);
            var order = objectsPool.GetObject<Order, Order.State>(new Order.State(sourcePlanet, new Percentage(1f)));
            order.RegisterMovement(Vector3.zero);
            order.Commit(targetPlanet ?? objectsPool.GetObject<Planet, Planet.State>(default));

            var army = new Army(objectsPool);
            army.SetState(new Army.State(order, magicNum, magicNum, faction, power,
                new Sun(new Position(Vector3.zero), 0.5f),
                fleetProperties, eventsCollector));
            return army;
        }

        public static Planet CreatePlanet(float production, Faction faction,
            params (AchievementId, int? level, AchievementType)[] achievements)
        {
            var (planetarySystemProperties, _) = PlayerPropertiesHelper.GetPlayerProperties(achievements);

            var planet = new Planet(ScriptableObject.CreateInstance<ObjectsPool>());

            planet.SetState(new Planet.State(faction, default, production, default, default, default,
                planetarySystemProperties));

            return planet;
        }
    }
}