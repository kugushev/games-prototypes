using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public interface IPlanetarySystem
    {
        Sun Sun { get; }
        IReadOnlyList<Planet> Planets { get; }
        bool TryGetEffects(Faction faction, [NotNullWhen(true)] out IPlanetarySystemEffects? effects);
        void SetDayOfYear(int dayOfYear);
    }

    public class PlanetarySystem : IPlanetarySystem
    {
        private Sun? _sun;
        private IReadOnlyList<Planet>? _planets;
        private IPlanetarySystemEffects? _planetarySystemEffects;

        internal void Init(Sun sun, IReadOnlyList<Planet> planets, IPlanetarySystemEffects planetarySystemEffects)
        {
            if (_sun != null || _planets != null)
                throw new SpaceFightException("Planetary System is already initialized");

            _sun = sun;
            _planets = planets;
            _planetarySystemEffects = planetarySystemEffects;
        }

        public Sun Sun => _sun ?? throw new SpaceFightException("Planetary System is not initialized");

        public IReadOnlyList<Planet> Planets =>
            _planets ?? throw new SpaceFightException("Planetary System is not initialized");


        public bool TryGetEffects(Faction faction, [NotNullWhen(true)] out IPlanetarySystemEffects? effects)
        {
            var planetarySystemEffects = _planetarySystemEffects ??
                                         throw new SpaceFightException("Planetary System is not initialized");

            if (planetarySystemEffects.ApplicantFaction == faction)
            {
                effects = planetarySystemEffects;
                return true;
            }

            effects = null;
            return false;
        }


        public void SetDayOfYear(int dayOfYear)
        {
            foreach (var planet in Planets)
                planet.SetDayOfYear(dayOfYear);
        }
    }
}