using System;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class MissionModel : PoolableOld<MissionModel.State>
    {
        public struct State
        {
            public State(MissionParameters missionParameters, PlanetarySystem planetarySystem, ConflictParty green,
                ConflictParty red, Faction playerFaction)
            {
                MissionParameters = missionParameters;
                PlanetarySystem = planetarySystem;
                Green = green;
                Red = red;
                PlayerFaction = playerFaction;
                ExecutionResult = null;
                DebriefingSummary = null;
            }

            public MissionParameters MissionParameters;
            public PlanetarySystem PlanetarySystem;
            public ConflictParty Green;
            public ConflictParty Red;
            public Faction PlayerFaction;
            public ExecutionResult? ExecutionResult;
            public DebriefingSummary? DebriefingSummary;
        }

        public MissionModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public MissionParameters Parameters => ObjectState.MissionParameters;

        public PlanetarySystem PlanetarySystem => ObjectState.PlanetarySystem;

        public ConflictParty Green => ObjectState.Green;

        public ConflictParty Red => ObjectState.Red;

        public Faction PlayerFaction => ObjectState.PlayerFaction;

        public ExecutionResult? ExecutionResult
        {
            get => ObjectState.ExecutionResult;
            set => ObjectState.ExecutionResult = value;
        }

        public DebriefingSummary? DebriefingSummary
        {
            get => ObjectState.DebriefingSummary;
            set => ObjectState.DebriefingSummary = value;
        }

        protected override void OnClear(State state)
        {
            state.PlanetarySystem.Dispose();
            state.Green.Dispose();
            state.Red.Dispose();
        }
    }
}