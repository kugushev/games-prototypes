using System;
using UnityEngine;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    [Serializable]
    public struct FleetProperties
    {
        [SerializeField] private float siegeMultiplier;
        [SerializeField] private float fightMultiplier;

        public FleetProperties(FleetPropertiesBuilder builder)
        {
            siegeMultiplier = builder.SiegeMultiplier;
            fightMultiplier = builder.FightMultiplier;
        }

        public float SiegeMultiplier => siegeMultiplier;

        public float FightMultiplier => fightMultiplier;
    }
}