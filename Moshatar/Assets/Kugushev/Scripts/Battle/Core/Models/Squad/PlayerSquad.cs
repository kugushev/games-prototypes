using System;
using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.AI;
using Kugushev.Scripts.Battle.Core.AI.Orders;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class PlayerSquad : IDisposable, IAgentsOwner
    {
        private const int SquadSize = 3;
        
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderAttack.Factory _orderAttackFactory;
        private readonly AgentsManager _agentsManager;

        private readonly ReactiveCollection<PlayerFighter> _units = new ReactiveCollection<PlayerFighter>();
        private PlayerFighter _selectedUnit;

        public PlayerSquad(
            OrderMove.Factory orderMoveFactory,
            OrderAttack.Factory orderAttackFactory,
            Battlefield battlefield,
            AgentsManager agentsManager)
        {
            _orderMoveFactory = orderMoveFactory;
            _orderAttackFactory = orderAttackFactory;
            _agentsManager = agentsManager;

            _agentsManager.Register(this);

            for (var index = 0; index < SquadSize; index++)
            {
                var character = new Character();

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
        }

        private void UnitOnHurt(PlayerFighter victim, BaseFighter attacker)
        {
            if (victim.CurrentOrder == null)
                victim.CurrentOrder = _orderAttackFactory.Create(attacker);
        }
    }
}