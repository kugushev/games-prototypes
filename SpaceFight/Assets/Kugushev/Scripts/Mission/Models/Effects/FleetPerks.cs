﻿using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Models.Effects
{
    [Serializable]
    public class FleetPerks : Poolable<FleetPerks.State>
    {
        [Serializable]
        public struct State
        {
            public ValuePipeline<Army> siegeDamage;
            public ValuePipeline<Army> fightDamage;
            public ValuePipeline<Army> fightProtection;
            public float deathStrike;
            public SiegeUltimatum ToNeutralPlanetUltimatum;

            public State(ValuePipeline<Army> siegeDamage, ValuePipeline<Army> fightDamage,
                ValuePipeline<Army> fightProtection)
            {
                this.siegeDamage = siegeDamage;
                this.fightDamage = fightDamage;
                this.fightProtection = fightProtection;
                deathStrike = 0;
                ToNeutralPlanetUltimatum = default;
            }
        }

        public IValuePipeline<Army> SiegeDamage => ObjectState.siegeDamage;
        public IValuePipeline<Army> FightDamage => ObjectState.fightDamage;
        public IValuePipeline<Army> FightProtection => ObjectState.fightProtection;
        public float DeathStrike => ObjectState.deathStrike;

        public ref readonly SiegeUltimatum GetToNeutralPlanetUltimatum() => ref ObjectState.ToNeutralPlanetUltimatum;

        public FleetPerks(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        protected override void OnClear(State state)
        {
            state.siegeDamage.Dispose();
            state.fightDamage.Dispose();
            state.fightProtection.Dispose();
        }
    }
}