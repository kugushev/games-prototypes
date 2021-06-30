﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Managers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class EnemySquad : ITickable, IDisposable, IAgentsOwner
    {
        private readonly PlayerSquad _playerSquad;
        private readonly SimpleAIService _simpleAIService;
        private readonly AgentsManager _agentsManager;

        private readonly ReactiveCollection<EnemyUnit> _units = new ReactiveCollection<EnemyUnit>();

        public EnemySquad(BattleManager battleManager, PlayerSquad playerSquad,
            SimpleAIService simpleAIService, Battlefield battlefield, AgentsManager agentsManager)
        {
            _playerSquad = playerSquad;
            _simpleAIService = simpleAIService;
            _agentsManager = agentsManager;

            _agentsManager.Register(this);

            for (var index = 0; index < battleManager.CurrentBattle.EnemyParty.Persons.Count; index++)
            {
                var row = BattleConstants.UnitsPositionsInRow[index];
                var point = new Vector2(BattleConstants.EnemySquadLine, row);

                var enemyUnit = new EnemyUnit(new Position(point), battlefield);
                _units.Add(enemyUnit);

                battlefield.RegisterUnt(enemyUnit);
            }
        }

        public IReadOnlyReactiveCollection<EnemyUnit> Units => _units;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units.Where(u => !u.IsDead))
            {
                if (enemyUnit.CurrentOrder is OrderAttack currentOrder)
                {
                    var toCurrentTarget = Vector2.Distance(
                        enemyUnit.Position.Value.Vector,
                        currentOrder.Target.Position.Value.Vector);
                    if (toCurrentTarget < enemyUnit.WeaponRange * BattleConstants.AIAggroResetMultiplier)
                        continue;
                }

                var order = _simpleAIService.AttackTheNearest(enemyUnit, _playerSquad.Units.Where(u => !u.IsDead));
                if (order != null)
                    enemyUnit.CurrentOrder = order;
            }
        }

        void IDisposable.Dispose() => _agentsManager.Unregister(this);
    }
}