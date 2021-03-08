using System;
using UnityEngine;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    [Serializable]
    public struct FleetProperties
    {
        [SerializeField] private int siegeMultiplier;
        [SerializeField] private int fightMultiplier;

        public FleetProperties(FleetPropertiesBuilder builder)
        {
            siegeMultiplier = builder.SiegeMultiplier;
            fightMultiplier = builder.FightMultiplier;
        }

        public int SiegeMultiplier => siegeMultiplier;

        public int FightMultiplier => fightMultiplier;
    }
}