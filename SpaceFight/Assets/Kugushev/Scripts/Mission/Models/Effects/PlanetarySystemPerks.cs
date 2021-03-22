using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Models.Effects
{
    [Serializable]
    public class PlanetarySystemPerks : Poolable<PlanetarySystemPerks.State>
    {
        [Serializable]
        public struct State
        {
            public Faction applicantFaction;
            public ValuePipeline<Planet> production;

            public State(Faction applicantFaction, ValuePipeline<Planet> production)
            {
                this.applicantFaction = applicantFaction;
                this.production = production;
            }
        }

        public PlanetarySystemPerks(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public Faction ApplicantFaction => ObjectState.applicantFaction;

        public IValuePipeline<Planet> Production => ObjectState.production;

        protected override void OnClear(State state)
        {
            state.production.Dispose();
        }
    }
}