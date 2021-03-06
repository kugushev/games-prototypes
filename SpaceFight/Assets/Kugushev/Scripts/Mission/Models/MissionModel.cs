using System;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    internal class MissionModel : IDisposable
    {
        [SerializeField] private MissionInfo missionInfo;
        [SerializeField] private PlanetarySystem planetarySystem;
        [SerializeField] private ConflictParty green;
        [SerializeField] private ConflictParty red;

        public MissionModel(MissionInfo missionInfo)
        {
            this.missionInfo = missionInfo;
        }

        public MissionInfo Info => missionInfo;

        public PlanetarySystem PlanetarySystem
        {
            get => planetarySystem;
            set => planetarySystem = value;
        }

        public ConflictParty Green
        {
            get => green;
            set => green = value;
        }

        public ConflictParty Red
        {
            get => red;
            set => red = value;
        }

        public void Dispose()
        {
            planetarySystem?.Dispose();
            green.Dispose();
            red.Dispose();
        }
    }
}