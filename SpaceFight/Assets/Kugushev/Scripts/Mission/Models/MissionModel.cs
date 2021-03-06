using System;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class MissionModel : IDisposable
    {
        [SerializeField] private MissionInfo missionInfo;
        private PlanetarySystem planetarySystem;
        private ConflictParty green;
        private ConflictParty red;

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

        public bool ReadyToExecute { get; set; }

        public void Dispose()
        {
            planetarySystem?.Dispose();
            green.Dispose();
            red.Dispose();
        }
    }
}