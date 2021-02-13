using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Common.Interfaces;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using Kugushev.Scripts.Game.Missions.Presets;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.AI.Tactical
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Simple AI")]
    public class SimpleAI : ScriptableObject, IAIAgent, ICommander
    {
        [SerializeField] private PlanetInfo[] planets;
        [SerializeField] private ObjectsPool objectsPool;

        [Serializable]
        public class PlanetInfo
        {
            [SerializeField] private PlanetPreset planet;
            [SerializeField] private Vector3 position;
            [SerializeField] private PlanetPreset[] neighbours;
            public PlanetPreset Planet => planet;
            public IReadOnlyList<PlanetPreset> Neighbours => neighbours;

            public Vector3 Position => position;
        }

        private readonly TempState _state = new TempState();

        private class TempState
        {
            public Fleet Fleet;
            public Faction AgentFaction;
        }

        #region ICommander

        public void AssignFleet(Fleet fleet, Faction faction)
        {
            _state.Fleet = fleet;
            _state.AgentFaction = faction;
        }

        public void WithdrawFleet()
        {
            _state.Fleet = null;
            _state.AgentFaction = Faction.Unspecified;
        }

        #endregion

        #region IAIAgent

        public void Act()
        {
            foreach (var planetInfo in planets)
            {
                if (planetInfo.Planet.Faction == _state.AgentFaction)
                {
                    if (planetInfo.Planet.Power > 5)
                        Act(planetInfo);
                }
            }
        }

        #endregion

        private void Act(PlanetInfo info)
        {
            // attack
            var weakestVictim = FindWeakest(info, faction => faction == _state.AgentFaction.GetOpposite() ||
                                                             faction == Faction.Neutral);
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
            var weakestAllay = FindWeakest(info, faction => faction == _state.AgentFaction);
            if (!ReferenceEquals(weakestAllay, null))
            {
                // todo: send reinforcements based on Random
                SendFleet(info, weakestAllay);
            }
        }

        private void SendFleet(PlanetInfo info, PlanetPreset weakestVictim)
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

            _state.Fleet.CommitOrder(order, weakestVictim);
        }

        private static PlanetPreset FindWeakest(PlanetInfo info, Predicate<Faction> predicate)
        {
            PlanetPreset weakest = null;

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

        private Vector3 GetPlanetPosition(PlanetPreset planet)
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