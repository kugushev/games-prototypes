using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Models.Abstractions;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "OrdersManager")]
    public class OrdersManager : Model
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private FleetManager fleetManager;
        [SerializeField] private HandType handHandType;
        [SerializeField] private float gapBetweenWaypoints = 0.05f;
        private readonly TempState _state = new TempState();

        private class TempState
        {
            public Order CurrentOrder;
            public Planet HighlightedPlanet;
        }

        [CanBeNull] public Order CurrentOrder => _state.CurrentOrder;

        public void HandlePlanetTouch(Planet planet)
        {
            if (_state.CurrentOrder == null)
            {
                if (planet.Faction == Faction.Player)
                {
                    _state.HighlightedPlanet = planet;
                }
            }
            else if (_state.CurrentOrder.SourcePlanet != planet)
            {
                fleetManager.CommitOrder(_state.CurrentOrder);
                _state.CurrentOrder = null;
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
                currentOrder.RegisterMovement(position, gapBetweenWaypoints);
            }
        }

        protected override void Dispose(bool destroying)
        {
            DropCurrentOrder();
        }
    }
}