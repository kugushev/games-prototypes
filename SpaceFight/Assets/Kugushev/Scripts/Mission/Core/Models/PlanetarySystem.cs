using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public interface IPlanetarySystem
    {
        Sun Sun { get; }
        IReadOnlyList<Planet> Planets { get; }
        void SetDayOfYear(int dayOfYear);
    }

    public class PlanetarySystem : IPlanetarySystem
    {
        private Sun? _sun;
        private List<Planet>? _planets;

        internal void Init(Sun sun, List<Planet> planets)
        {
            if (_sun != null || _planets != null)
                throw new SpaceFightException("Planetary System is already initialized");

            _sun = sun;
            _planets = planets;
        }

        public Sun Sun => _sun ?? throw new SpaceFightException("Planetary System is not initialized");

        public IReadOnlyList<Planet> Planets =>
            _planets ?? throw new SpaceFightException("Planetary System is not initialized");

        public void SetDayOfYear(int dayOfYear)
        {
            foreach (var planet in Planets)
                planet.SetDayOfYear(dayOfYear);
        }
    }
}