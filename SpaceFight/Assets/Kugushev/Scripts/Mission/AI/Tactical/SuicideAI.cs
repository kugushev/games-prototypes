using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.AI.Tactical
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(SuicideAI))]
    public class SuicideAI : ScriptableObject, IAIAgent, ICommander
    {
        [SerializeField] private MissionModelProvider missionModelProvider;
        [SerializeField] private Pathfinder pathfinder;
        [SerializeField] private ObjectsPool objectsPool;
        private const float ArmyRadius = 5f * 0.02f;

        [NonSerialized] private Fleet _fleet;
        [NonSerialized] private Faction _agentFaction;

        public void AssignFleet(Fleet fleet, Faction faction)
        {
            _fleet = fleet;
            _agentFaction = faction;
        }

        public void WithdrawFleet()
        {
            _fleet = default;
            _agentFaction = default;
        }

        public void Act()
        {
            if (missionModelProvider.TryGetModel(out var missionModel))
            {
                var planetarySystem = missionModel.PlanetarySystem;
                foreach (var planet in planetarySystem.Planets)
                {
                    if (planet.Faction == _agentFaction && planet.Power > 0)
                        Act(planet, planetarySystem);
                }
            }
        }

        private void Act(Planet from, PlanetarySystem planetarySystem)
        {
            var to = FindNeutralOrEnemyPlanet(planetarySystem);
            if (to == null)
                return;

            var order = objectsPool.GetObject<Order, Order.State>(new Order.State(from, new Percentage(1f)));

            // go to sun
            var sun = planetarySystem.GetSun();
            pathfinder.FindPath(from.Position, sun.Position, ArmyRadius, (p, o) => o.RegisterMovement(p.Point), order);

            // go to target
            pathfinder.FindPath(sun.Position, to.Position, ArmyRadius, (p, o) => o.RegisterMovement(p.Point), order);

            _fleet.CommitOrder(order, to);
        }

        [CanBeNull]
        private Planet FindNeutralOrEnemyPlanet(PlanetarySystem planetarySystem)
        {
            foreach (var planet in planetarySystem.Planets)
            {
                if (planet.Faction == Faction.Neutral)
                    return planet;
            }

            foreach (var planet in planetarySystem.Planets)
            {
                if (planet.Faction == _agentFaction.GetOpposite())
                    return planet;
            }

            return null;
        }
    }
}