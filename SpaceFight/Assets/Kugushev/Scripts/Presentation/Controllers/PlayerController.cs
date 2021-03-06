using System;
using Kugushev.Scripts.Common.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Player;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private OrdersManager leftOrders;
        [SerializeField] private OrdersManager rightOrders;

        public void HandleTouchPlanet(HandController sender, Planet planet)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandlePlanetTouch(planet);

            planet.Selected = true;
        }

        public void HandleDetouchPlanet(HandController sender, Planet planet)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandlePlanetDetouch();
            planet.Selected = false;
        }

        public void HandleSelect(HandController sender)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandleSelect(sender.ArmyPowerAllocated);
        }

        public void HandleDeselect(HandController sender)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandleDeselect();
        }

        public void HandleMove(HandController sender, Vector3 position)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandleMove(position);
        }

        private OrdersManager GetOrdersManager(HandController handController) => handController.HandType switch
        {
            HandType.Right => rightOrders,
            HandType.Left => leftOrders,
            _ => throw new ArgumentOutOfRangeException(nameof(handController.HandType), handController.HandType,
                "Unexpected hand type")
        };
    }
}