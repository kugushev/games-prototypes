using System;
using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle;
using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Kugushev.Scripts.Presentation.Battle.Presenters;
using Kugushev.Scripts.Presentation.Battle.Presenters.Units;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Controllers
{
    public class SquadController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default!;

        [Inject] private PlayerSquad _playerSquad = default!;
        [Inject] private OrderMove.Factory _orderMoveFactory = default!;
        [Inject] private OrderAttack.Factory _orderAttackFactory = default!;

        private readonly List<PlayerUnitPresenter> _units = new List<PlayerUnitPresenter>(4);

        private bool _uglyHackLockInput = false;

        private void Update()
        {
            if (_uglyHackLockInput)
            {
                _uglyHackLockInput = false;
                return;
            }

            // todo: replace with collider on surface to prevent race conditions in orders
            if (_currentUnit is { } && Input.GetMouseButtonDown(1))
            {
                OrderMove(_currentUnit);
            }
        }

        private void FixedUpdate() => _playerSquad.ProcessOrders(new DeltaTime(Time.fixedDeltaTime));

        private void OrderMove(PlayerUnitPresenter unit)
        {
            Vector2 target = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if ((target - unit.Unit.Position.Value.Vector).sqrMagnitude < BattleConstants.UnitToTargetEpsilon)
                return;

            unit.Unit.CurrentOrder = _orderMoveFactory.Create(new Position(target));
        }

        public void EnemyUnitClicked(EnemyUnit enemy)
        {
            if (_currentUnit is { })
            {
                // order attack
                _currentUnit.Unit.CurrentOrder = _orderAttackFactory.Create(enemy);

                _uglyHackLockInput = true;
            }
        }

        #region Squad Unit Selection

        private PlayerUnitPresenter? _currentUnit;

        public void UnitSelected(PlayerUnitPresenter? unit)
        {
            _currentUnit = unit;
            if (unit == null)
                return;


        }

        public void Register(PlayerUnitPresenter unit)
        {
            // todo: invert it!
            //_playerSquad.Add(unit.Unit!);
            _units.Add(unit);
        }

        public void Unregister(PlayerUnitPresenter unit) => _units.Remove(unit);

        #endregion
    }
}