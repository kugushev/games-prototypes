using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public abstract class BaseWayfarer
    {
        private readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<UnitDirection> _direction =
            new ReactiveProperty<UnitDirection>(UnitDirection.Down);

        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<UnitDirection> Direction => _direction;
    }
}