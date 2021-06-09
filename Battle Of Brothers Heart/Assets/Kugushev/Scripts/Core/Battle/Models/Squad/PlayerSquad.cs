﻿using System;
using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Kugushev.Scripts.Core.Game.Models;
using Kugushev.Scripts.Core.Game.Parameters;
using UniRx;

namespace Kugushev.Scripts.Core.Battle.Models.Squad
{
    public class PlayerSquad : BaseSquad, IDisposable
    {
        private readonly IInputController _inputController;
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderAttack.Factory _orderAttackFactory;

        private readonly ReactiveCollection<PlayerUnit> _units = new ReactiveCollection<PlayerUnit>();
        private PlayerUnit? _selectedUnit;

        public PlayerSquad(BattleParameters battleParameters,
            IInputController inputController,
            OrderMove.Factory orderMoveFactory,
            OrderAttack.Factory orderAttackFactory)
        {
            _inputController = inputController;
            _orderMoveFactory = orderMoveFactory;
            _orderAttackFactory = orderAttackFactory;

            _inputController.PlayerUnitSelected += OnPlayerUnitSelected;
            _inputController.EnemyUnitCommand += OnEnemyUnitCommand;
            _inputController.GroundCommand += OnGroundCommand;

            foreach (var _ in battleParameters.Team)
            {
                _units.Add(new PlayerUnit());
            }
        }

        public IReadOnlyReactiveCollection<PlayerUnit> Units => _units;

        protected override IReadOnlyList<BaseUnit> BaseUnits => _units;

        private void OnPlayerUnitSelected(PlayerUnit? unit)
        {
            _selectedUnit?.Deselect();
            unit?.Select();
            _selectedUnit = unit;
        }

        private void OnEnemyUnitCommand(EnemyUnit target)
        {
            if (_selectedUnit == null)
                return;

            _selectedUnit.CurrentOrder = _orderAttackFactory.Create(target);
        }

        private void OnGroundCommand(Position target)
        {
            if (_selectedUnit == null)
                return;

            var toTargetVector = target.Vector - _selectedUnit.Position.Value.Vector;
            if (toTargetVector.sqrMagnitude < BattleConstants.UnitToTargetEpsilon)
                return;

            _selectedUnit.CurrentOrder = _orderMoveFactory.Create(target);
        }

        void IDisposable.Dispose()
        {
            _inputController.PlayerUnitSelected -= OnPlayerUnitSelected;
            _inputController.EnemyUnitCommand -= OnEnemyUnitCommand;
            _inputController.GroundCommand -= OnGroundCommand;
        }
    }
}