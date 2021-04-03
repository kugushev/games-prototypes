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
using Kugushev.Scripts.Mission.ValueObjects;
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
            if (ObjectState.PlanetarySystemPerks.TryGetPerks(Faction, out var perks))
                return perks.production.Calculate(ObjectState.production, this);
            return ObjectState.production;
        }

        public bool TryRecruit(Percentage powerToRecruit, out int armyPower)
        {
            if (powerToRecruit.Amount == 0f)
            {
                armyPower = 0;
                return false;
            }

            var powerToRecruitAbs = Mathf.FloorToInt(ObjectState.power * powerToRecruit.Amount);

            if (powerToRecruitAbs > GameplayConstants.SoftCapArmyPower)
                powerToRecruitAbs = GameplayConstants.SoftCapArmyPower;

            if (powerToRecruitAbs < 1)
                powerToRecruitAbs = 1;

            if (ObjectState.power - powerToRecruitAbs < 0)
            {
                Debug.LogWarning($"Lack of power for {powerToRecruitAbs}. Planet has {ObjectState.power}");
                armyPower = 0;
                return false;
            }

            if (!ObjectState.PlanetarySystemPerks.TryGetPerks(Faction, out var perks) ||
                perks.IsFreeRecruitment?.Invoke(powerToRecruitAbs) != true)
                ObjectState.power -= powerToRecruitAbs;


            armyPower = powerToRecruitAbs;
            return true;
        }

        public void Reinforce(Army army)
        {
            ObjectState.power += army.Power;
        }

        public FightRoundResult SufferFightRound(Faction enemyFaction, float damage, Army enemy)
        {
            ObjectState.power -= damage;

            if (ObjectState.power < 0)
            {
                var previousOwner = ObjectState.faction;

                ObjectState.power *= -1;
                ObjectState.faction = enemyFaction;

                ObjectState.EventsCollector.PlanetCaptured.Add(
                    new PlanetCaptured(ObjectState.EventsCollector.Elapsed,
                        enemyFaction, previousOwner, ObjectState.power + enemy.Power));

                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        public bool Consider(in SiegeUltimatum ultimatum, Army sender)
        {
            if (!ultimatum.Initialized)
                return false;

            if (ObjectState.faction == Faction.Neutral)
            {
                if (sender.Power >= ObjectState.power + ultimatum.Predominance)
                {
                    var previousOwner = ObjectState.faction;
                    var previousPower = ObjectState.power;
                    // surrender
                    ObjectState.power *= ultimatum.Surrendered.Amount;
                    ObjectState.faction = sender.Faction;

                    ObjectState.EventsCollector.PlanetCaptured.Add(
                        new PlanetCaptured(ObjectState.EventsCollector.Elapsed,
                            sender.Faction, previousOwner, sender.Power - previousPower));
                    return true;
                }
            }

            return false;
        }

        public float GetDamage()
        {
            if (ObjectState.PlanetarySystemPerks.TryGetPerks(Faction, out var perks))
                return perks.damage.Calculate(GameplayConstants.UnifiedDamage, this);

            return GameplayConstants.UnifiedDamage;
        }

        private void UpdatePosition() => ObjectState.position =
            ObjectState.orbit.ToPosition(ObjectState.sun.Position, ObjectState.dayOfYear);
    }
}