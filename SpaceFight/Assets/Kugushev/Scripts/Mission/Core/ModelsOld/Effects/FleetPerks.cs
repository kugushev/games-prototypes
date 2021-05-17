using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Models.Effects
{
    [Serializable]
    public class FleetPerks : PoolableOld<FleetPerks.State>
    {
        [Serializable]
        public struct State
        {
            public ValuePipelineOld<Army> SiegeDamage;
            public ValuePipelineOld<Army> FightDamage;
            public ValuePipelineOld<Army> FightProtection;
            public ValuePipelineOld<(Planet target, Faction playerFaction)> ArmySpeed;
            public float deathStrike;
            public SiegeUltimatum ToNeutralPlanetUltimatum;

            public State(ValuePipelineOld<Army> siegeDamage, ValuePipelineOld<Army> fightDamage,
                ValuePipelineOld<Army> fightProtection, ValuePipelineOld<(Planet target, Faction playerFaction)> armySpeed)
            {
                SiegeDamage = siegeDamage;
                FightDamage = fightDamage;
                FightProtection = fightProtection;
                ArmySpeed = armySpeed;
                deathStrike = 0;
                ToNeutralPlanetUltimatum = default;
            }
        }

        public IValuePipeline<Army> SiegeDamage => ObjectState.SiegeDamage;
        public IValuePipeline<Army> FightDamage => ObjectState.FightDamage;
        public IValuePipeline<Army> FightProtection => ObjectState.FightProtection;
        public IValuePipeline<(Planet target, Faction playerFaction)> ArmySpeed => ObjectState.ArmySpeed;
        public float DeathStrike => ObjectState.deathStrike;

        public ref readonly SiegeUltimatum GetToNeutralPlanetUltimatum() => ref ObjectState.ToNeutralPlanetUltimatum;

        public FleetPerks(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        protected override void OnClear(State state)
        {
            state.SiegeDamage.Dispose();
            state.FightDamage.Dispose();
            state.FightProtection.Dispose();
        }
    }
}