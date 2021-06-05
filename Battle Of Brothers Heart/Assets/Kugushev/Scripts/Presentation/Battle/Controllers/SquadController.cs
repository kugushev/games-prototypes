using System;
using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle;
using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Kugushev.Scripts.Presentation.Battle.Presenters;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Controllers
{
    public class SquadController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default!;

        [Inject] private Squad _squad = default!;
        [Inject] private OrderMove.Factory _orderMoveFactory = default!;
        [Inject] private OrderAttack.Factory _orderAttackFactory = default!;

        private readonly List<SquadUnitPresenter> _units = new List<SquadUnitPresenter>(4);

        private bool _uglyHackLockInput = false;

        private void Update()
        {
            if (_uglyHackLockInput)
            {
                _uglyHackLockInput = false;
                return;
            }

            // todo: replace with collider on surface to prevent race conditions in orders
            if (_currentUnit is { } && Input.GetMouseButtonDown(0))
            {
                OrderMove(_currentUnit);
            }
        }

        private void FixedUpdate() => _squad.ProcessOrders(new DeltaTime(Time.fixedDeltaTime));

        private void OrderMove(SquadUnitPresenter unit)
        {
            Vector2 target = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if ((target - unit.Model.Position.Value.Vector).sqrMagnitude < BattleConstants.UnitToTargetEpsilon)
                return;

            unit.Model.CurrentOrder = _orderMoveFactory.Create(new Position(target));
        }

        public void EnemyUnitClicked(EnemyUnit enemy)
        {
            if (_currentUnit is { })
            {
                // order attack
                _currentUnit.Model.CurrentOrder = _orderAttackFactory.Create(enemy);

                _uglyHackLockInput = true;
            }
        }

        #region Squad Unit Selection

        private SquadUnitPresenter? _currentUnit;

        public void UnitSelected(SquadUnitPresenter? unit)
        {
            _currentUnit = unit;
            if (unit == null)
                return;

            foreach (var u in _units)
                if (u != unit)
                    u.Deselect();
        }

        public void Register(SquadUnitPresenter unit)
        {
            // todo: invert it!
            _squad.Add(unit.Model!);
            _units.Add(unit);
        }

        public void Unregister(SquadUnitPresenter unit) => _units.Remove(unit);

        #endregion
    }
}