using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Game.AI.Tactical
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Simple AI")]
    public class SimpleAI : ScriptableObject
    {
        [SerializeField] private FleetManager fleet;
        [SerializeField] private PlanetInfo[] planets;
        [SerializeField] private ObjectsPool objectsPool;

        [Serializable]
        public class PlanetInfo
        {
            [SerializeField] private Planet planet;
            [SerializeField] private Vector3 position;
            [SerializeField] private Planet[] neighbours;
            public Planet Planet => planet;
            public IReadOnlyList<Planet> Neighbours => neighbours;

            public Vector3 Position => position;
        }

        public void Act()
        {
            foreach (var planetInfo in planets)
            {
                if (planetInfo.Planet.Faction == Faction.Enemy)
                {
                    if (planetInfo.Planet.Power > 5)
                        Act(planetInfo);
                }
            }
        }

        private void Act(PlanetInfo info)
        {
            // attack
            var weakestVictim = FindWeakest(info, faction => faction == Faction.Player || faction == Faction.Neutral);
            if (!ReferenceEquals(weakestVictim, null))
            {
                if (info.Planet.Power > weakestVictim.Power + 6)
                {
                    // todo: send invaders based on Random

                    SendFleet(info, weakestVictim);
                }
                else if (info.Planet.Power >= GameConstants.SoftCapArmyPower)
                {
                    SendFleet(info, weakestVictim);
                }

                // we're not sending reinforcements if have enemies
                return;
            }

            // send reinforcements
            var weakestAllay = FindWeakest(info, faction => faction == Faction.Enemy);
            if (!ReferenceEquals(weakestAllay, null))
            {
                // todo: send reinforcements based on Random
                SendFleet(info, weakestAllay);
            }
        }

        private void SendFleet(PlanetInfo info, Planet weakestVictim)
        {
            var order = objectsPool.GetObject<Order, Order.State>(new Order.State(info.Planet));

            var from = info.Position;
            var to = GetPlanetPosition(weakestVictim);

            var stepSize = 0.001f;
            for (float i = 0; i < 1f; i += stepSize)
            {
                var point = Vector3.Lerp(@from, to, i);
                order.RegisterMovement(point, 0.05f);
            }
            
            fleet.CommitOrder(order, weakestVictim);
        }

        private static Planet FindWeakest(PlanetInfo info, Predicate<Faction> predicate)
        {
            Planet weakest = null;

            foreach (var neighbour in info.Neighbours)
            {
                if (predicate(neighbour.Faction))
                {
                    if (weakest == null)
                        weakest = neighbour;
                    else if (neighbour.Power < weakest.Power)
                        weakest = neighbour;
                }
            }

            return weakest;
        }

        private Vector3 GetPlanetPosition(Planet planet)
        {
            foreach (var planetInfo in planets)
            {
                if (planetInfo.Planet.Equals(planet))
                    return planetInfo.Position;
            }

            Debug.LogError($"Unable to find planet {planet}");
            return Vector3.zero;
        }
    }
}