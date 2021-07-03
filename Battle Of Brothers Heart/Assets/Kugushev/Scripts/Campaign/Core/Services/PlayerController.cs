using System;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core.Services
{
    internal class PlayerController : IInitializable, IDisposable
    {
        private readonly InputController _inputController;
        private readonly Wayfarers _wayfarers;
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderVisitCity.Factory _visitCityFactory;
        private readonly OrderAttackBandit.Factory _attackBanditFactory;

        public PlayerController(InputController inputController, Wayfarers wayfarers,
            OrderMove.Factory orderMoveFactory, OrderVisitCity.Factory visitCityFactory,
            OrderAttackBandit.Factory attackBanditFactory)
        {
            _inputController = inputController;
            _wayfarers = wayfarers;
            _orderMoveFactory = orderMoveFactory;
            _visitCityFactory = visitCityFactory;
            _attackBanditFactory = attackBanditFactory;
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
            _wayfarers.Player.CurrentOrder = _orderMoveFactory.Create(target);
        }

        private void OnInteractableRightClick(IInteractable interactable)
        {
            switch (interactable)
            {
                case City city:
                    _wayfarers.Player.CurrentOrder = _visitCityFactory.Create(city);
                    break;
                case BanditWayfarer bandit:
                    _wayfarers.Player.CurrentOrder = _attackBanditFactory.Create(bandit);
                    break;
                default:
                    Debug.LogError($"Invalid interactable {interactable}");
                    break;
            }
        }
    }
}