using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    public readonly struct SiegeUltimatum
    {
        public SiegeUltimatum(Percentage surrendered, float predominance)
        {
            Surrendered = surrendered;
            Predominance = predominance;
            Initialized = true;
        }

        public Percentage Surrendered { get; }
        public float Predominance { get; }
        public bool Initialized { get; }
    }
}