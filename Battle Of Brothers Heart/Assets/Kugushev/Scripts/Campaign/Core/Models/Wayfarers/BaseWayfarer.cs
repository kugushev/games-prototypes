using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.AI;
using UniRx;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public abstract class BaseWayfarer : IAgent
    {
        private readonly ReactiveProperty<Position> _position = new ReactiveProperty<Position>();

        private readonly ReactiveProperty<Direction2d> _direction =
            new ReactiveProperty<Direction2d>(Common.Core.Enums.Direction2d.Down);

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