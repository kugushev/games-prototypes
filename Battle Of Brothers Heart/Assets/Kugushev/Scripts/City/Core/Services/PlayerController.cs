using System;
using Kugushev.Scripts.City.Core.Models;
using Kugushev.Scripts.City.Core.Models.Villagers;
using Kugushev.Scripts.City.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.City.Core.Services
{
    internal class PlayerController : IInitializable, IDisposable
    {
        private readonly InputController _inputController;
        private readonly PlayerVillager _playerVillager;
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderGoToRoadSign.Factory _goToRoadSignFactory;

        public PlayerController(InputController inputController, PlayerVillager playerVillager,
            OrderMove.Factory orderMoveFactory, OrderGoToRoadSign.Factory goToRoadSignFactory)
        {
            _inputController = inputController;
            _playerVillager = playerVillager;
            _orderMoveFactory = orderMoveFactory;
            _goToRoadSignFactory = goToRoadSignFactory;
        }

        public void Initialize()
        {
            _inputController.InteractableRightClick += OnInteractableRightClick;
            _inputController.GroundRightClick += OnGroundRightClick;
        }

        public void Dispose()
        {
            _inputController.InteractableRightClick -= OnInteractableRightClick;
            _inputController.GroundRightClick -= OnGroundRightClick;
        }

        private void OnGroundRightClick(Position target)
        {
            _playerVillager.CurrentOrder = _orderMoveFactory.Create(target);
        }

        private void OnInteractableRightClick(IInteractable interactable)
        {
            switch (interactable)
            {
                case RoadSign city:
                    _playerVillager.CurrentOrder = _goToRoadSignFactory.Create(city);
                    break;
                default:
                    Debug.LogError($"Invalid interactable {interactable}");
                    break;
            }
        }
    }
}