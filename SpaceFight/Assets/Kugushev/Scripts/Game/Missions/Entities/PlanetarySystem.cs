using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Missions.Presets;

namespace Kugushev.Scripts.Game.Missions.Entities
{
    public class PlanetarySystem : Poolable<PlanetarySystem.State>
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
    }
}