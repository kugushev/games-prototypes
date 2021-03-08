using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class MissionModel : Poolable<MissionModel.State>
    {
        public struct State
        {
            public State(MissionInfo missionInfo, PlanetarySystem planetarySystem, ConflictParty green,
                ConflictParty red)
            {
                MissionInfo = missionInfo;
                PlanetarySystem = planetarySystem;
                Green = green;
                Red = red;
                ExecutionResult = null;
                DebriefingInfo = null;
            }

            public MissionInfo MissionInfo;
            public PlanetarySystem PlanetarySystem;
            public ConflictParty Green;
            public ConflictParty Red;
            public ExecutionResult? ExecutionResult;
            [CanBeNull] public DebriefingInfo DebriefingInfo;
        }

        public MissionModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public MissionInfo Info => ObjectState.MissionInfo;

        public PlanetarySystem PlanetarySystem
        {
            get => ObjectState.PlanetarySystem;
            set => ObjectState.PlanetarySystem = value;
        }

        public ConflictParty Green
        {
            get => ObjectState.Green;
            set => ObjectState.Green = value;
        }

        public ConflictParty Red
        {
            get => ObjectState.Red;
            set => ObjectState.Red = value;
        }

        public ExecutionResult? ExecutionResult
        {
            get => ObjectState.ExecutionResult;
            set => ObjectState.ExecutionResult = value;
        }

        [CanBeNull]
        public DebriefingInfo DebriefingInfo
        {
            get => ObjectState.DebriefingInfo;
            set => ObjectState.DebriefingInfo = value;
        }

        protected override void OnClear(State state)
        {
            state.PlanetarySystem?.Dispose();
            state.Green.Dispose();
            state.Red.Dispose();
        }
    }
}