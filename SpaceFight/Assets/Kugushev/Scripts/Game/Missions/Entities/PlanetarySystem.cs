using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Missions.Presets;

namespace Kugushev.Scripts.Game.Missions.Entities
{
    public class PlanetarySystem : Poolable<PlanetarySystem.State>
    {
        public readonly struct State
        {
            public State(float sunScale)
            {
                SunScale = sunScale;
            }

            public float SunScale { get; }
        }

        private readonly List<PlanetPreset> _planets = new List<PlanetPreset>();

        public PlanetarySystem(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public IReadOnlyList<PlanetPreset> Planets => _planets;
        public void AddPlanet(PlanetPreset planet) => _planets.Add(planet);
        public float SunScale => ObjectState.SunScale;

        protected override void OnClear(State state)
        {
            foreach (var planet in _planets)
            {
                planet.Reset();
            }
            
            _planets.Clear();
        }

        protected override void OnRestore(State state) => _planets.Clear();
    }
}