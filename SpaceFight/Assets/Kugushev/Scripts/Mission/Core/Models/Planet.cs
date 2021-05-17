using System;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using UniRx;
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

        //todo: MissionEventsCollector eventsCollector, PlanetarySystemPerks planetarySystemPerks)
        public Planet(Faction faction, PlanetSize size, Production production, Orbit orbit, Power startPower,
            IPlanetarySystem planetarySystem)
        {
            _faction.Value = faction;
            Size = size;
            _production = production;
            _orbit = orbit;
            _power.Value = startPower.Amount;
            _planetarySystem = planetarySystem;
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

        private Position CalculatePosition(int dayOfYear) =>
            _orbit.ToPosition(_planetarySystem.Sun.Position, dayOfYear);

        public class Factory : PlaceholderFactory<Faction, PlanetSize, Production, Orbit, Power, Planet>
        {
        }
    }
}