using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ValueObjects;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public class Planet : IFighter
    {
        private readonly ReactiveProperty<Faction> _faction = new ReactiveProperty<Faction>();
        private readonly Production _production;
        private readonly Orbit _orbit;
        private readonly ReactiveProperty<float> _power = new ReactiveProperty<float>();
        private readonly ReactiveProperty<int> _dayOfYear = new ReactiveProperty<int>(0);
        private readonly Lazy<IReadOnlyReactiveProperty<Position>> _position;

        private readonly IPlanetarySystem _planetarySystem;
        private readonly EventsCollectingService _eventsCollectingService;

        public Planet(Faction faction, PlanetSize size, Production production, Orbit orbit, Power startPower,
            IPlanetarySystem planetarySystem, EventsCollectingService eventsCollectingService)
        {
            _faction.Value = faction;
            Size = size;
            _production = production;
            _orbit = orbit;
            _power.Value = startPower.Amount;
            _planetarySystem = planetarySystem;
            _eventsCollectingService = eventsCollectingService;
            _position = new Lazy<IReadOnlyReactiveProperty<Position>>(() =>
                _dayOfYear.Select(CalculatePosition).ToReactiveProperty());
        }

        public PlanetSize Size { get; }
        public IReadOnlyReactiveProperty<Position> Position => _position.Value;
        public IReadOnlyReactiveProperty<Faction> Faction => _faction;
        public IReadOnlyReactiveProperty<float> Power => _power;

        public void SetDayOfYear(int dayOfYear) => _dayOfYear.Value = dayOfYear;

        #region IFighter

        bool IFighter.Active => true; // todo: remove?
        Position IFighter.Position => Position.Value;
        Faction IFighter.Faction => _faction.Value;
        bool IFighter.CanBeAttacked => true;

        #endregion

        public UniTask ExecuteProductionCycle()
        {
            if (Faction.Value == Enums.Faction.Neutral)
                return UniTask.CompletedTask;

            _power.Value += CalculateProductionIncrement();
            return UniTask.CompletedTask;
        }

        private float CalculateProductionIncrement()
        {
            if (_planetarySystem.TryGetEffects(Faction.Value, out var effects))
                return effects.Production.Calculate(_production.Amount, this);
            return _production.Amount;
        }

        public bool TryRecruit(Percentage powerToRecruit, out int armyPower)
        {
            if (powerToRecruit.Amount == 0f)
            {
                armyPower = 0;
                return false;
            }

            var powerToRecruitAbs = Mathf.FloorToInt(_power.Value * powerToRecruit.Amount);

            if (powerToRecruitAbs > GameplayConstants.SoftCapArmyPower)
                powerToRecruitAbs = GameplayConstants.SoftCapArmyPower;

            if (powerToRecruitAbs < 1)
                powerToRecruitAbs = 1;

            if (_power.Value - powerToRecruitAbs < 0)
            {
                Debug.LogWarning($"Lack of power for {powerToRecruitAbs}. Planet has {_power.Value}");
                armyPower = 0;
                return false;
            }

            if (!_planetarySystem.TryGetEffects(Faction.Value, out var perks) ||
                perks.IsFreeRecruitment?.Invoke(powerToRecruitAbs) != true)
                _power.Value -= powerToRecruitAbs;


            armyPower = powerToRecruitAbs;
            return true;
        }

        public void Reinforce(Army army)
        {
            _power.Value += army.Power;
        }

        public FightRoundResult SufferFightRound(Faction enemyFaction, float damage, Army enemy)
        {
            var power = _power.Value;

            power -= damage;

            if (power < 0)
            {
                var previousOwner = Faction.Value;

                power *= -1;
                _faction.Value = enemyFaction;

                _eventsCollectingService.PlanetCaptured.Add(
                    new PlanetCaptured(_eventsCollectingService.Elapsed,
                        enemyFaction, previousOwner, power + enemy.Power));

                return FightRoundResult.Defeated;
            }

            _power.Value = power;

            return FightRoundResult.StillAlive;
        }

        public bool Consider(in SiegeUltimatum ultimatum, Army sender)
        {
            if (!ultimatum.Initialized)
                return false;

            if (Faction.Value == Enums.Faction.Neutral)
            {
                if (sender.Power >= _power.Value + ultimatum.Predominance)
                {
                    var previousOwner = Faction.Value;
                    var previousPower = _power.Value;
                    // surrender
                    _power.Value *= ultimatum.Surrendered.Amount;
                    _faction.Value = sender.Faction;

                    _eventsCollectingService.PlanetCaptured.Add(
                        new PlanetCaptured(_eventsCollectingService.Elapsed,
                            sender.Faction, previousOwner, sender.Power - previousPower));
                    return true;
                }
            }

            return false;
        }

        public float GetDamage()
        {
            if (_planetarySystem.TryGetEffects(Faction.Value, out var perks))
                return perks.Damage.Calculate(GameplayConstants.UnifiedDamage, this);

            return GameplayConstants.UnifiedDamage;
        }

        private Position CalculatePosition(int dayOfYear) =>
            _orbit.ToPosition(_planetarySystem.Sun.Position, dayOfYear);


        public class Factory : PlaceholderFactory<Faction, PlanetSize, Production, Orbit, Power, Planet>
        {
        }
    }
}