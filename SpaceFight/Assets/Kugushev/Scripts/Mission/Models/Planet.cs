using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class Planet : Poolable<Planet.State>, IModel, IFighter
    {
        [Serializable]
        public struct State
        {
            public State(Faction faction, PlanetSize size, int production, Orbit orbit, Sun sun,
                MissionEventsCollector eventsCollector)
            {
                this.faction = faction;
                this.size = size;
                this.production = production;
                this.orbit = orbit;
                this.sun = sun;
                EventsCollector = eventsCollector;
                power = 0;
                selected = false;
                dayOfYear = 0;
                position = orbit.ToPosition(sun.Position, 0);
            }

            public Faction faction;
            public PlanetSize size;
            public int production;
            public Orbit orbit;
            public Sun sun;
            public int power;
            public bool selected;
            public int dayOfYear;
            public Position position;
            public readonly MissionEventsCollector EventsCollector;
        }

        public Planet(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public Faction Faction => ObjectState.faction;

        public bool CanBeAttacked => true;

        public PlanetSize Size => ObjectState.size;

        public int Power => ObjectState.power;

        public bool Selected
        {
            get => ObjectState.selected;
            set => ObjectState.selected = value;
        }

        public int DayOfYear
        {
            get => ObjectState.dayOfYear;
            set
            {
                ObjectState.dayOfYear = value;
                UpdatePosition();
            }
        }

        public Position Position => ObjectState.position;

        public UniTask ExecuteProductionCycle()
        {
            if (Faction == Faction.Neutral && ObjectState.power >= GameplayConstants.NeutralPlanetMaxPower)
                return UniTask.CompletedTask;

            ObjectState.power += ObjectState.production;
            return UniTask.CompletedTask;
        }

        public int Recruit(Percentage powerToRecruit)
        {
            var powerToRecruitAbs = Mathf.FloorToInt(ObjectState.power * powerToRecruit.Amount);

            if (powerToRecruitAbs > GameplayConstants.SoftCapArmyPower)
                powerToRecruitAbs = GameplayConstants.SoftCapArmyPower;

            ObjectState.power -= powerToRecruitAbs;
            return powerToRecruitAbs;
        }

        public void Reinforce(Army army)
        {
            ObjectState.power += army.Power;
        }

        public FightRoundResult SufferFightRound(Faction enemyFaction, int damage = GameplayConstants.UnifiedDamage)
        {
            ObjectState.power -= damage;

            if (ObjectState.power < 0)
            {
                var previousOwner = ObjectState.faction;

                ObjectState.power *= -1;
                ObjectState.faction = enemyFaction;

                ObjectState.EventsCollector?.PlanetCaptured(enemyFaction, previousOwner, ObjectState.power);

                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        private void UpdatePosition() => ObjectState.position =
            ObjectState.orbit.ToPosition(ObjectState.sun.Position, ObjectState.dayOfYear);
    }
}