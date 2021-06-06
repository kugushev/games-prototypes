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

        private readonly List<SquadBaseUnitPresenter> _units = new List<SquadBaseUnitPresenter>(4);

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

        private void FixedUpdate() => _squad.ProcessOrders(new DeltaTime(Time.fixedDeltaTime));

        private void OrderMove(SquadBaseUnitPresenter baseUnit)
        {
            Vector2 target = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if ((target - baseUnit.Unit.Position.Value.Vector).sqrMagnitude < BattleConstants.UnitToTargetEpsilon)
                return;

            baseUnit.Unit.CurrentOrder = _orderMoveFactory.Create(new Position(target));
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

        private SquadBaseUnitPresenter? _currentUnit;

        public void UnitSelected(SquadBaseUnitPresenter? unit)
        {
            _currentUnit = unit;
            if (unit == null)
                return;

            foreach (var u in _units)
                if (u != unit)
                    u.Deselect();
        }

        public void Register(SquadBaseUnitPresenter baseUnit)
        {
            // todo: invert it!
            _squad.Add(baseUnit.Unit!);
            _units.Add(baseUnit);
        }

        public void Unregister(SquadBaseUnitPresenter baseUnit) => _units.Remove(baseUnit);

        #endregion
    }
}