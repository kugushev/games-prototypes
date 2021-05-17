using System.Collections.Generic;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Core.Specifications
{
    public class PlanetSpecsRegistry
    {
        public IReadOnlyList<PlanetSpec> SmallPlanets { get; } = new[]
        {
            new PlanetSpec(PlanetSize.Mercury, 1, 3)
        };

        public IReadOnlyList<PlanetSpec> MediumPlanets { get; } = new[]
        {
            new PlanetSpec(PlanetSize.Mars, 3, 5),
            new PlanetSpec(PlanetSize.Earth, 4, 6)
        };

        public IReadOnlyList<PlanetSpec> BigPlanets { get; } = new[]
        {
            new PlanetSpec(PlanetSize.Uranus, 6, 9),
            new PlanetSpec(PlanetSize.Saturn, 9, 12),
            new PlanetSpec(PlanetSize.Jupiter, 12, 15)
        };
    }
}