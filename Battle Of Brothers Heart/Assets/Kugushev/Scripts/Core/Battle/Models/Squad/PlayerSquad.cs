using System;
using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Kugushev.Scripts.Core.Game.Models;
using UniRx;

namespace Kugushev.Scripts.Core.Battle.Models.Squad
{
    public class PlayerSquad : BaseSquad<PlayerUnit>, IDisposable
    {
        private readonly IInputController _inputController;
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderAttack.Factory _orderAttackFactory;

        private PlayerUnit? _selectedUnit;

        public PlayerSquad(PlayerTeam playerTeam,
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

            foreach (var teamMember in playerTeam.TeamMembers)
            {
                Units.Add(new PlayerUnit());
            }
        }

        public IReadOnlyReactiveCollection<PlayerUnit> PlayerUnits => Units;

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