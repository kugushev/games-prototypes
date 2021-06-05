using System;
using System.Collections.Generic;
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

        private readonly List<SquadUnitPresenter> _units = new List<SquadUnitPresenter>(4);

        private void Update()
        {
            if (_currentUnit is { } && Input.GetMouseButtonDown(0))
            {
                OrderMove(_currentUnit);
            }
        }

        private void FixedUpdate() => _squad.ProcessOrders(new DeltaTime(Time.fixedDeltaTime));

        private void OrderMove(SquadUnitPresenter unit)
        {
            var target = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            unit.Model.CurrentOrder = _orderMoveFactory.Create(new Position(target));
        }

        #region Unit Selection

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