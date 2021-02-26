using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Entities
{
    [Serializable]
    public class Planet : Poolable<Planet.State>, IModel, IFighter
    {
        [Serializable]
        public struct State
        {
            public State(Faction faction, PlanetSize size, int production, Orbit orbit, Sun sun)
            {
                this.faction = faction;
                this.size = size;
                this.production = production;
                this.orbit = orbit;
                Sun = sun;
                power = 0;
                selected = false;
                dayOfYear = 0;
                position = orbit.ToPosition(sun.Position, 0);
            }

            public Faction faction;
            public PlanetSize size;
            public int production;
            public Orbit orbit;
            public Sun Sun;
            public int power;
            public bool selected;
            public int dayOfYear;
            public Position position;
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
            if (Faction == Faction.Neutral && ObjectState.power >= GameConstants.NeutralPlanetMaxPower)
                return UniTask.CompletedTask;

            ObjectState.power += ObjectState.production;
            return UniTask.CompletedTask;
        }

        public int Recruit(Percentage powerToRecruit)
        {
            var powerToRecruitAbs = Mathf.FloorToInt(ObjectState.power * powerToRecruit.Amount);

            if (powerToRecruitAbs > GameConstants.SoftCapArmyPower)
                powerToRecruitAbs = GameConstants.SoftCapArmyPower;

            ObjectState.power -= powerToRecruitAbs;
            return powerToRecruitAbs;


            // if (ObjectState.Power <= GameConstants.SoftCapArmyPower)
            // {
            //     var allPower = ObjectState.Power;
            //     ObjectState.Power = 0;
            //     return allPower;
            // }
            //
            // ObjectState.Power -= GameConstants.SoftCapArmyPower;
            // return GameConstants.SoftCapArmyPower;
        }

        public void Reinforce(Army army)
        {
            ObjectState.power += army.Power;
        }

        public FightRoundResult SufferFightRound(Faction enemyFaction, int damage = GameConstants.UnifiedDamage)
        {
            ObjectState.power -= damage;

            if (ObjectState.power < 0)
            {
                ObjectState.power *= -1;
                ObjectState.faction = enemyFaction;
                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        private void UpdatePosition() => ObjectState.position =
            ObjectState.orbit.ToPosition(ObjectState.Sun.Position, ObjectState.dayOfYear);
    }
}