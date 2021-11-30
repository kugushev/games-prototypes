using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.AI;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class EnemySquad : ITickable, IDisposable, IAgentsOwner
    {
        private const int SquadSize = 3;

        private readonly PlayerSquad _playerSquad;
        private readonly SimpleAIService _simpleAIService;
        private readonly AgentsManager _agentsManager;

        private readonly ReactiveCollection<EnemyFighter> _units = new ReactiveCollection<EnemyFighter>();
        
        public static readonly IReadOnlyList<float> UnitsPositionsInRow = new[] { -20f, -10f, 10f, 20f };

        public EnemySquad(PlayerSquad playerSquad,
            SimpleAIService simpleAIService, Battlefield battlefield, AgentsManager agentsManager)
        {
            _playerSquad = playerSquad;
            _playerSquad.EnemySquad = this;

            _simpleAIService = simpleAIService;
            _agentsManager = agentsManager;

            _agentsManager.Register(this);

            for (var index = 0; index < SquadSize; index++)
            {
                var character = new Character();

                var row = UnitsPositionsInRow[index];
                var point = new Vector2(BattleConstants.EnemySquadLine, row);

                var enemyUnit = new EnemyFighter(new Position(point), character, battlefield);
                _units.Add(enemyUnit);

                battlefield.RegisterUnt(enemyUnit);
            }
        }

        public IReadOnlyReactiveCollection<EnemyFighter> Units => _units;

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