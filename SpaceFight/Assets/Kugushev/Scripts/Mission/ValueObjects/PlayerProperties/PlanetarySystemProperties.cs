using System;
using Kugushev.Scripts.Mission.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    [Serializable]
    public struct PlanetarySystemProperties
    {
        [SerializeField] private PlanetarySystemPropertiesBuilder planetarySystemBuilder;

        public PlanetarySystemProperties(Faction applyToFaction, PlanetarySystemPropertiesBuilder planetarySystemBuilder)
        {
            ApplyToFaction = applyToFaction;
            this.planetarySystemBuilder = planetarySystemBuilder;
        }

        public Faction ApplyToFaction { get; }
        public float? LowProductionMultiplier => planetarySystemBuilder.LowProductionMultiplier;
        public float? AboveLowProductionMultiplier => planetarySystemBuilder.AboveLowProductionMultiplier;
        public float? LowProductionCap => planetarySystemBuilder.LowProductionCap;
    }
}