using System;

namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    [Serializable]
    public struct PlanetarySystemPropertiesBuilder
    {
        public float? LowProductionMultiplier { get; set; }
        public float? AboveLowProductionMultiplier { set; get; }
        public float? LowProductionCap { get; set; }
    }
}