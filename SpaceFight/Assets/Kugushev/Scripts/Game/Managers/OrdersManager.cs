using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "OrdersManager")]
    public class OrdersManager : ScriptableObject
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private HandType handHandType;
        private readonly TempState _state = new TempState();

        private class TempState
        {
            public Order CurrentOrder;
            public Planet HighlightedPlanet;
        }


        public void HandlePlanetTouch(Planet planet)
        {
            if (_state.CurrentOrder == null)
            {
                if (planet.Faction == Faction.Player)
                {
                    _state.HighlightedPlanet = planet;
                }
            }
            else
            {
                // todo: commit order (no reason to wait trigger release)
            }
        }

        public void HandlePlanetDetouch()
        {
            if (_state.CurrentOrder == null)
                _state.HighlightedPlanet = null;
            else
                _state.CurrentOrder.Status = OrderStatus.Assignment;
        }

        public void HandleSelect()
        {
            if (_state.HighlightedPlanet != null)
            {
                DropCurrentOrder();
                _state.CurrentOrder = pool.GetObject<Order, Order.State>(new Order.State(_state.HighlightedPlanet));
            }
        }

        public void HandleDeselect()
        {
            DropCurrentOrder();
        }

        private void DropCurrentOrder()
        {
            if (_state.CurrentOrder != null)
            {
                var order = _state.CurrentOrder;
                order.Dispose();

                _state.CurrentOrder = null;
            }
        }

        public void HandleMove(Vector3 position)
        {
            var currentOrder = _state.CurrentOrder;
            if (currentOrder != null && currentOrder.Status == OrderStatus.Assignment)
            {
                currentOrder.RegisterMovement(position);
            }
        }
    }
}