using System;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Core.Models.WorldUnits
{
    public abstract class WorldUnit
    {
        private DateTime _unfreezeTime;

        public WorldUnit(Position position, Direction2d direction, Party party)
        {
            Position = position;
            Direction = direction;
            Party = party;
        }

        public Position Position { get; private set; }
        public Direction2d Direction { get; private set; }
        public Party Party { get; }

        public bool IsFrozen => _unfreezeTime > DateTime.Now;

        public void Freeze(TimeSpan seconds) => _unfreezeTime = DateTime.Now + seconds;

        public void SubscribeTo(IObservable<Position> property)
            => property.Subscribe(v => Position = v);

        public void SubscribeTo(IObservable<Direction2d> property)
            => property.Subscribe(v => Direction = v);
    }
}