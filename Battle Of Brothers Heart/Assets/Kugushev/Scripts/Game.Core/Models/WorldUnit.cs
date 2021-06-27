using System;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class WorldUnit
    {
        public WorldUnit(Position position, Direction2d direction)
        {
            Position = position;
            Direction = direction;
        }

        public Position Position { get; private set; }
        public Direction2d Direction { get; private set; }

        public void SubscribeTo(IObservable<Position> property)
            => property.Subscribe(v => Position = v);

        public void SubscribeTo(IObservable<Direction2d> property)
            => property.Subscribe(v => Direction = v);
    }
}