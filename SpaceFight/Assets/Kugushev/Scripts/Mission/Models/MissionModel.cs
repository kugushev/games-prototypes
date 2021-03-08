using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class MissionModel : Poolable<MissionModel.State>
    {
        public struct State
        {
            public State(MissionInfo missionInfo, PlanetarySystem planetarySystem, ConflictParty green,
                ConflictParty red, Faction playerFaction)
            {
                MissionInfo = missionInfo;
                PlanetarySystem = planetarySystem;
                Green = green;
                Red = red;
                PlayerFaction = playerFaction;
                ExecutionResult = null;
                DebriefingSummary = null;
            }

            public MissionInfo MissionInfo;
            public PlanetarySystem PlanetarySystem;
            public ConflictParty Green;
            public ConflictParty Red;
            public Faction PlayerFaction;
            public ExecutionResult? ExecutionResult;
            [CanBeNull] public DebriefingSummary DebriefingSummary;
        }

        public MissionModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public MissionInfo Info => ObjectState.MissionInfo;

        public PlanetarySystem PlanetarySystem => ObjectState.PlanetarySystem;

        public ConflictParty Green => ObjectState.Green;

        public ConflictParty Red => ObjectState.Red;

        public Faction PlayerFaction => ObjectState.PlayerFaction;

        public ExecutionResult? ExecutionResult
        {
            get => ObjectState.ExecutionResult;
            set => ObjectState.ExecutionResult = value;
        }

        public DebriefingSummary DebriefingSummary
        {
            get => ObjectState.DebriefingSummary;
            set => ObjectState.DebriefingSummary = value;
        }

        protected override void OnClear(State state)
        {
            state.PlanetarySystem?.Dispose();
            state.Green.Dispose();
            state.Red.Dispose();
        }
    }
}