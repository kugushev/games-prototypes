using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Tools;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.AI.Tactical
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(SimpleAI))]
    public class SimpleAI : ScriptableObject, IAIAgent, ICommander
    {
        [SerializeField] private MissionModelProvider? missionModelProvider;
        [SerializeField] private ObjectsPool? objectsPool;
        [SerializeField] private Pathfinder? pathfinder;
        [SerializeField] private int neighboursCount = 2;
        [SerializeField] private int minPowerToAct = 15;
        [SerializeField] private float recruitment = 0.75f;
        [SerializeField] private float forceRecruitCap = GameplayConstants.SoftCapArmyPower;
        private const float ArmyRadius = 5f * 0.02f;

        private readonly TempState _state = new TempState();

        private class TempState
        {
            public Fleet? Fleet;
            public Faction AgentFaction;
            public readonly List<Planet> NeighboursPlanetsBuffer = new List<Planet>(16);
            public readonly PlanetsDistanceComparer PlanetsDistanceComparer = new PlanetsDistanceComparer();
        }

        private Percentage DefaultRecruitment => new Percentage(recruitment);

        #region ICommander

        public bool Surrendered => false;

        public void AssignFleet(Fleet fleet, Faction faction)
        {
            _state.Fleet = fleet;
            _state.AgentFaction = faction;
        }

        public void WithdrawFleet()
        {
            _state.Fleet = default;
            _state.AgentFaction = default;
        }

        #endregion

        #region IAIAgent

        public void Act()
        {
            Asserting.NotNull(missionModelProvider);

            if (missionModelProvider.TryGetModel(out var missionModel))
            {
                var planetarySystem = missionModel.PlanetarySystem;
                foreach (var planet in planetarySystem.Planets)
                {
                    if (planet.Faction == _state.AgentFaction)
                    {
                        if (planet.Power > minPowerToAct)
                            Act(planet, planetarySystem);
                    }
                }
            }
        }

        #endregion

        private void Act(Planet planet, PlanetarySystem planetarySystem)
        {
            FillWithNearestPlanets(planet, planetarySystem, neighboursCount, _state.NeighboursPlanetsBuffer,
                _state.PlanetsDistanceComparer);

            // attack
            var weakestVictim = FindWeakest(planet, _state.NeighboursPlanetsBuffer,
                faction => faction == _state.AgentFaction.GetOpposite() || faction == Faction.Neutral);

            if (weakestVictim is { })
            {
                if (planet.Power * DefaultRecruitment.Amount > weakestVictim.Power + 6)
                {
                    // todo: send invaders based on Random

                    SendFleet(planet, weakestVictim);
                }
                else if (planet.Power * DefaultRecruitment.Amount >= forceRecruitCap)
                {
                    SendFleet(planet, weakestVictim);
                }
            }

            _state.NeighboursPlanetsBuffer.Clear();
        }

        private void SendFleet(Planet planet, Planet weakestVictim)
        {
            Asserting.NotNull(objectsPool, pathfinder, _state.Fleet);

            var order = objectsPool.GetObject<Order, Order.State>(new Order.State(planet, DefaultRecruitment));

            var from = planet.Position;
            var to = weakestVictim.Position;

            var pathIsValid = pathfinder.FindPath(from, to, ArmyRadius,
                (p, o) => o.RegisterMovement(p.Point), order);

            if (pathIsValid)
                _state.Fleet.CommitOrder(order, weakestVictim);
            else
            {
                Debug.LogError("Can't send fleet: path is too long");
                order.Dispose();
            }
        }

        private static Planet? FindWeakest(Planet exceptPlanet, IReadOnlyCollection<Planet> planets,
            Predicate<Faction> predicate)
        {
            Planet? weakest = null;

            foreach (var planet in planets)
            {
                if (planet == exceptPlanet)
                    continue;

                if (predicate(planet.Faction))
                {
                    if (weakest == null)
                        weakest = planet;
                    else if (planet.Power < weakest.Power)
                        weakest = planet;
                }
            }

            return weakest;
        }

        private void FillWithNearestPlanets(Planet from, PlanetarySystem planetarySystem, int top,
            List<Planet> buffer, PlanetsDistanceComparer comparer)
        {
            if (top > planetarySystem.Planets.Count - 1)
                return;

            Asserting.NotNull(pathfinder);

            // Prepare
            buffer.Clear();
            foreach (var planet in planetarySystem.Planets)
            {
                if (planet == from)
                    continue;

                buffer.Add(planet);
            }

            comparer.Setup(from, pathfinder, ArmyRadius);

            // Sort
            buffer.Sort(comparer);
            
            comparer.TearDown();
        }
    }
}