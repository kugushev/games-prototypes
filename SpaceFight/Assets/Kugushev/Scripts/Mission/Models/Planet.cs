using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class Planet : Poolable<Planet.State>, IModel, IFighter
    {
        [Serializable]
        public struct State
        {
            public State(Faction faction, PlanetSize size, float production, Orbit orbit, Sun sun,
                MissionEventsCollector eventsCollector, PlanetarySystemPerks planetarySystemPerks, float power)
            {
                this.faction = faction;
                this.size = size;
                this.production = production;
                this.orbit = orbit;
                this.sun = sun;
                EventsCollector = eventsCollector;
                PlanetarySystemPerks = planetarySystemPerks;
                this.power = power;
                selected = false;
                dayOfYear = 0;
                position = orbit.ToPosition(sun.Position, 0);
            }

            public Faction faction;
            public PlanetSize size;
            public float production;
            public Orbit orbit;
            public Sun sun;
            public float power;
            public bool selected;
            public int dayOfYear;
            public Position position;
            public readonly MissionEventsCollector EventsCollector;
            public readonly PlanetarySystemPerks PlanetarySystemPerks;
        }

        public Planet(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public Faction Faction => ObjectState.faction;

        public bool CanBeAttacked => true;

        public PlanetSize Size => ObjectState.size;

        public float Power => ObjectState.power;

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
            if (Faction == Faction.Neutral)
                return UniTask.CompletedTask;

            ObjectState.power += CalculateProductionIncrement();
            return UniTask.CompletedTask;
        }

        private float CalculateProductionIncrement()
        {
            if (ObjectState.PlanetarySystemPerks.ApplicantFaction == Faction)
            {
                return ObjectState.PlanetarySystemPerks.Production.Calculate(ObjectState.production, this);
            }
            return ObjectState.production;
        }

        public bool TryRecruit(Percentage powerToRecruit, out int armyPower)
        {
            var powerToRecruitAbs = Mathf.FloorToInt(ObjectState.power * powerToRecruit.Amount);

            if (powerToRecruitAbs > GameplayConstants.SoftCapArmyPower)
                powerToRecruitAbs = GameplayConstants.SoftCapArmyPower;

            if (powerToRecruitAbs < 1) 
                powerToRecruitAbs = 1;

            if (ObjectState.power - powerToRecruitAbs < 0)
            {
                Debug.LogError($"Lack of power for {powerToRecruitAbs}. Planet has {ObjectState.power}");
                armyPower = 0;
                return false;
            }

            ObjectState.power -= powerToRecruitAbs;
            
            armyPower = powerToRecruitAbs;
            return true;
        }

        public void Reinforce(Army army)
        {
            ObjectState.power += army.Power;
        }

        public FightRoundResult SufferFightRound(Faction enemyFaction, float damage = GameplayConstants.UnifiedDamage)
        {
            ObjectState.power -= damage;

            if (ObjectState.power < 0)
            {
                var previousOwner = ObjectState.faction;

                ObjectState.power *= -1;
                ObjectState.faction = enemyFaction;

                ObjectState.EventsCollector.PlanetCaptured.Add(
                    new PlanetCaptured(enemyFaction, previousOwner, ObjectState.power));

                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        private void UpdatePosition() => ObjectState.position =
            ObjectState.orbit.ToPosition(ObjectState.sun.Position, ObjectState.dayOfYear);
    }
}