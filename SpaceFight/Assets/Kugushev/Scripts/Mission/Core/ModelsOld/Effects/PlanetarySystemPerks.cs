using System;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Models.Effects
{
    [Serializable]
    public class PlanetarySystemPerks : PoolableOld<PlanetarySystemPerks.State>
    {
        [Serializable]
        public struct State
        {
            public Faction applicantFaction;
            public ValuePipelineOld<Planet> Production;
            public ValuePipelineOld<Planet> Damage;
            public Func<float, bool>? IsFreeRecruitment;
            public Func<bool>? GetExtraPlanetOnStart;

            public State(Faction applicantFaction, ValuePipelineOld<Planet> production, ValuePipelineOld<Planet> damage)
            {
                this.applicantFaction = applicantFaction;
                Production = production;
                Damage = damage;
                IsFreeRecruitment = null;
                GetExtraPlanetOnStart = null;
            }
        }

        public PlanetarySystemPerks(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public bool TryGetPerks(Faction faction, out State perks)
        {
            if (ObjectState.applicantFaction == faction)
            {
                perks = ObjectState;
                return true;
            }

            perks = default;
            return false;
        }

        public bool IsFreeRecruitment(float powerToRecruit) =>
            ObjectState.IsFreeRecruitment?.Invoke(powerToRecruit) ?? false;

        protected override void OnClear(State state)
        {
            state.Production.Dispose();
        }
    }
}