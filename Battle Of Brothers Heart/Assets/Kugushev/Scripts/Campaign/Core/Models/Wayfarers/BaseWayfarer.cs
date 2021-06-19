using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Interfaces.AI;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public abstract class BaseWayfarer : IAgent
    {
        private readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<Direction2d> _direction =
            new ReactiveProperty<Direction2d>(Direction2d.Down);

        public IReadOnlyReactiveProperty<Position> Position => _position;
        public IReadOnlyReactiveProperty<Direction2d> Direction => _direction;

        #region IAgent

        public IOrder? CurrentOrder { get; set; }

        public void ProcessCurrentOrder(DeltaTime delta)
        {
            
        }

        #endregion
    }
}