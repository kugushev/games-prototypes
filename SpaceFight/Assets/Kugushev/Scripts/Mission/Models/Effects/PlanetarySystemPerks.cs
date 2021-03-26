using System;
using JetBrains.Annotations;
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
            [CanBeNull] public Func<float, bool> IsFreeRecruitment;

            public State(Faction applicantFaction, ValuePipeline<Planet> production)
            {
                this.applicantFaction = applicantFaction;
                this.production = production;
                IsFreeRecruitment = null;
            }
        }

        public PlanetarySystemPerks(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public Faction ApplicantFaction => ObjectState.applicantFaction;

        public IValuePipeline<Planet> Production => ObjectState.production;

        public bool IsFreeRecruitment(float powerToRecruit) =>
            ObjectState.IsFreeRecruitment?.Invoke(powerToRecruit) ?? false;

        protected override void OnClear(State state)
        {
            state.production.Dispose();
        }
    }
}