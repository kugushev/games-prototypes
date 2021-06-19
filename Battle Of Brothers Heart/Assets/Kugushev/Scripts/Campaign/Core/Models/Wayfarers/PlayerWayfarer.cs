using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Game.Core.Models.AI.Orders;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class PlayerWayfarer : BaseWayfarer
    {
        private readonly InputController _inputController;
        private readonly OrderMove.Factory _orderMoveFactory;

        public PlayerWayfarer(Position position, InputController inputController, OrderMove.Factory orderMoveFactory) :
            base(position)
        {
            _inputController = inputController;
            _orderMoveFactory = orderMoveFactory;

            // todo: unsubscribe
            _inputController.GroundCommand += OnGroundCommand;
        }

        private void OnGroundCommand(Position target)
        {
            CurrentOrder = _orderMoveFactory.Create(target);
        }
    }
}