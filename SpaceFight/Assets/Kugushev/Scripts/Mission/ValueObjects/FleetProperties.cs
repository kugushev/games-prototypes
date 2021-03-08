using System;
using UnityEngine;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    [Serializable]
    public struct FleetProperties
    {
        [SerializeField] private int siegeMultiplier;

        public FleetProperties(FleetPropertiesBuilder builder)
        {
            siegeMultiplier = builder.SiegeMultiplier;
        }

        public int SiegeMultiplier => siegeMultiplier;
    }
}