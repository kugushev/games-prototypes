using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.AI.Tactical
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Simple AI")]
    public class SimpleAI : ScriptableObject
    {
        [SerializeField] private FleetManager fleet;
        [SerializeField] private PlanetInfo[] planets;

        [Serializable]
        public class PlanetInfo
        {
            [SerializeField] private Planet planet;
            [SerializeField] private Planet[] neighbours;
            public Planet Planet => planet;
            public IReadOnlyList<Planet> Neighbours => neighbours;
        }

        public void Act()
        {
            foreach (var planetInfo in planets)
            {
                if (planetInfo.Planet.Faction == Faction.Enemy)
                {
                    Act(planetInfo);
                }
            }
        }

        private void Act(PlanetInfo info)
        {
            // attack
            var weakestVictim = FindWeakest(info, faction => faction == Faction.Player || faction == Faction.Neutral);
            if (weakestVictim != null)
            {
                if (info.Planet.Power > weakestVictim.Power)
                {
                    // todo: send invaders based on Random
                }
                else if (info.Planet.Power >= GameConstants.SoftCapArmyPower)
                {
                    // todo: send invaders 100% chance
                }
            }


            // send reinforcements
            var weakestAllay = FindWeakest(info, faction => faction == Faction.Enemy);
            if (weakestAllay != null)
            {
                // todo: send reinforcements based on Random 
            }
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
    }
}