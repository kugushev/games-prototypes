using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Core.ValueObjects
{
    public readonly struct PlanetSpec
    {
        public PlanetSpec(PlanetSize size, int minProduction, int maxProduction)
        {
            Size = size;
            MinProduction = minProduction;
            MaxProduction = maxProduction;
        }

        public PlanetSize Size { get; }

        public int MinProduction { get; }

        public int MaxProduction { get; }
    }
}