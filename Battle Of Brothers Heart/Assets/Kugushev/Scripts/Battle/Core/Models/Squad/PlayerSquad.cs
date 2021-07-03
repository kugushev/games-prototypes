using System;
using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Managers;
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

        private readonly ReactiveCollection<PlayerFighter> _units = new ReactiveCollection<PlayerFighter>();
        private PlayerFighter? _selectedUnit;

        public PlayerSquad(BattleManager battleManager,
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

            for (var index = 0; index < battleManager.CurrentBattleSafe.Player.Party.Characters.Count; index++)
            {
                var character = battleManager.CurrentBattleSafe.Player.Party.Characters[index];

                var row = BattleConstants.UnitsPositionsInRow[index];
                var point = new Vector2(BattleConstants.PlayerSquadLine, row);

                var playerUnit = new PlayerFighter(new Position(point), character, battlefield);
                playerUnit.Hurt += attacker => UnitOnHurt(playerUnit, attacker);
                _units.Add(playerUnit);

                battlefield.RegisterUnt(playerUnit);
            }
        }

        public IReadOnlyReactiveCollection<PlayerFighter> Units => _units;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;

        private void OnPlayerUnitSelected(PlayerFighter? unit)
        {
            _selectedUnit?.Deselect();
            unit?.Select();
            _selectedUnit = unit;
        }

        private void OnEnemyUnitCommand(EnemyFighter target)
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

        private void UnitOnHurt(PlayerFighter victim, BaseFighter attacker)
        {
            if (victim.CurrentOrder == null)
                victim.CurrentOrder = _orderAttackFactory.Create(attacker);
        }
    }
}