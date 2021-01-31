using System;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models;
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
            
            print("HandleTouchPlanet");
        }

        public void HandleDetouchPlanet(HandController sender, Planet planet)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandlePlanetDetouch();
            planet.Selected = false;
            print("HandleDetouchPlanet");
        }

        public void HandleSelect(HandController sender)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandleSelect();
            print("HandleSelect");
        }

        public void HandleDeselect(HandController sender)
        {
            var ordersManager = GetOrdersManager(sender);
            ordersManager.HandleDeselect();
            print("HandleDeselect");
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