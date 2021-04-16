﻿using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Models.Effects
{
    [Serializable]
    public class FleetPerks : Poolable<FleetPerks.State>
    {
        [Serializable]
        public struct State
        {
            public ValuePipeline<Army> SiegeDamage;
            public ValuePipeline<Army> FightDamage;
            public ValuePipeline<Army> FightProtection;
            public ValuePipeline<(Planet target, Faction playerFaction)> ArmySpeed;
            public float deathStrike;
            public SiegeUltimatum ToNeutralPlanetUltimatum;

            public State(ValuePipeline<Army> siegeDamage, ValuePipeline<Army> fightDamage,
                ValuePipeline<Army> fightProtection, ValuePipeline<(Planet target, Faction playerFaction)> armySpeed)
            {
                this.SiegeDamage = siegeDamage;
                this.FightDamage = fightDamage;
                this.FightProtection = fightProtection;
                this.ArmySpeed = armySpeed;
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