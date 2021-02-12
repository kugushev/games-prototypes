using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.Entities
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

        private readonly List<Planet> _planets = new List<Planet>();

        public PlanetarySystem(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public List<Planet> Planets => _planets;
        public void AddPlanet(Planet planet) => Planets.Add(planet);
        public float SunScale => ObjectState.SunScale;

        protected override void OnClear(State state) => Planets.Clear();
        protected override void OnRestore(State state) => Planets.Clear();
    }
}