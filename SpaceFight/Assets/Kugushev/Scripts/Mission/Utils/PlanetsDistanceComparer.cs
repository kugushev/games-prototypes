using System.Collections.Generic;
using Kugushev.Scripts.Mission.AI;
using Kugushev.Scripts.Mission.Entities;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Utils
{
    public class PlanetsDistanceComparer : IComparer<Planet>
    {
        private Planet _rootPlanet;
        private Pathfinder _pathfinder;
        private float _armyRadius;

        public void Setup(Planet rootPlanet, Pathfinder pathfinder, float armyRadius)
        {
            _rootPlanet = rootPlanet;
            _pathfinder = pathfinder;
            _armyRadius = armyRadius;
        }

        public void TearDown()
        {
            _rootPlanet = null;
            _pathfinder = null;
        }

        public int Compare(Planet x, Planet y)
        {
            if (_pathfinder == null || _rootPlanet == null)
            {
                Debug.LogError($"{nameof(PlanetsDistanceComparer)} didn't setup");
                return 0;
            }

            if (x == null || y == null)
                return 0;

            var validX = _pathfinder.FindPath(_rootPlanet.Position, x.Position, _armyRadius, out var lengthX);
            var validY = _pathfinder.FindPath(_rootPlanet.Position, y.Position, _armyRadius, out var lengthY);
            
            if (validX && validY)
                return lengthX - lengthY;

            if (validX)
                return -1;

            if (validY)
                return 1;

            return 0;
        }
    }
}