﻿using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Core.Models;

namespace Kugushev.Scripts.Mission.Models
{
    public class PlanetarySystem : PoolableOld<PlanetarySystem.State>
    {
        public readonly struct State
        {
            public State(Sun sun)
            {
                Sun = sun;
            }

            public readonly Sun Sun;
        }

        private readonly List<Planet> _planets = new List<Planet>();

        public PlanetarySystem(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public IReadOnlyList<Planet> Planets => _planets;
        public void AddPlanet(Planet planet) => _planets.Add(planet);
        public ref readonly Sun GetSun() => ref ObjectState.Sun;

        protected override void OnClear(State state)
        {
            foreach (var planet in _planets)
                planet.Dispose();

            _planets.Clear();
        }

        protected override void OnRestore(State state) => _planets.Clear();

        public void SetDayOfYear(int dayOfYear)
        {
            foreach (var planet in _planets)
                planet.DayOfYear = dayOfYear;
        }
    }
}