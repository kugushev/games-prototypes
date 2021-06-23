using System;
using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Parameters;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class PlayerSquad : IDisposable, IAgentsOwner
    {
        private readonly IInputController _inputController;
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderAttack.Factory _orderAttackFactory;
        private readonly AgentsManager _agentsManager;

        private readonly ReactiveCollection<PlayerUnit> _units = new ReactiveCollection<PlayerUnit>();
        private PlayerUnit? _selectedUnit;

        public PlayerSquad(BattleParameters battleParameters,
            IInputController inputController,
            OrderMove.Factory orderMoveFactory,
            OrderAttack.Factory orderAttackFactory,
            Battlefield battlefield,
            AgentsManager agentsManager)
        {
            _inputController = inputController;
            _orderMoveFactory = orderMoveFactory;
            _orderAttackFactory = orderAttackFactory;
            _agentsManager = agentsManager;

            _agentsManager.Register(this);
            _inputController.PlayerUnitSelected += OnPlayerUnitSelected;
            _inputController.EnemyUnitCommand += OnEnemyUnitCommand;
            _inputController.GroundCommand += OnGroundCommand;

            for (var index = 0; index < battleParameters.Team.Count; index++)
            {
                var row = BattleConstants.UnitsPositionsInRow[index];
                var point = new Vector2(BattleConstants.PlayerSquadLine, row);

                var playerUnit = new PlayerUnit(new Position(point), battlefield);
                playerUnit.Hurt += attacker => UnitOnHurt(playerUnit, attacker);
                _units.Add(playerUnit);

                battlefield.RegisterUnt(playerUnit);
            }
        }

        public IReadOnlyReactiveCollection<PlayerUnit> Units => _units;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;

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
            _agentsManager.Unregister(this);
            _inputController.PlayerUnitSelected -= OnPlayerUnitSelected;
            _inputController.EnemyUnitCommand -= OnEnemyUnitCommand;
            _inputController.GroundCommand -= OnGroundCommand;
        }

        private void UnitOnHurt(PlayerUnit victim, BaseUnit attacker)
        {
            if (victim.CurrentOrder == null)
                victim.CurrentOrder = _orderAttackFactory.Create(attacker);
        }
    }
}