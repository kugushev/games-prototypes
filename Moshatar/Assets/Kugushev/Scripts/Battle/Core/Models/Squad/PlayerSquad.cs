using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.AI;
using Kugushev.Scripts.Battle.Core.AI.Orders;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class PlayerSquad : IDisposable, IAgentsOwner, ITickable
    {
        public EnemySquad EnemySquad; // todo: fix this hack later
        private readonly OrderMove.Factory _orderMoveFactory;
        private readonly OrderAttack.Factory _orderAttackFactory;
        private readonly Battlefield _battlefield;
        private readonly AgentsManager _agentsManager;
        private readonly SimpleAIService _simpleAIService;
        private readonly BattleGameplayManager _battleGameplayManager;

        private readonly ReactiveCollection<PlayerFighter> _units = new ReactiveCollection<PlayerFighter>();

        public PlayerSquad(
            OrderMove.Factory orderMoveFactory,
            OrderAttack.Factory orderAttackFactory,
            Battlefield battlefield,
            AgentsManager agentsManager,
            SimpleAIService simpleAIService,
            BattleGameplayManager battleGameplayManager)
        {
            _orderMoveFactory = orderMoveFactory;
            _orderAttackFactory = orderAttackFactory;
            _battlefield = battlefield;
            _agentsManager = agentsManager;
            _simpleAIService = simpleAIService;
            _battleGameplayManager = battleGameplayManager;

            _agentsManager.Register(this);

            // for (var index = 0; index < _battleGameplayManager.Parameters.PlayerSquadSize; index++)
            // {
            //     var character = new Character(_battleGameplayManager.Parameters.TeammateDefaultMaxHp,
            //         _battleGameplayManager.Parameters.TeammateDefaultDamage);
            //
            //     var row = BattleConstants.UnitsPositionsInRow[index];
            //     var point = new Vector2(BattleConstants.PlayerSquadLine, row);
            //
            //     var playerUnit = new PlayerFighter(new Position(point), character, battlefield);
            //     // playerUnit.Hurt += attacker => UnitOnHurt(playerUnit, attacker);
            //     _units.Add(playerUnit);
            //
            //     battlefield.RegisterUnt(playerUnit);
            // }

            Hero = new HeroFighter(new Position(Vector2.zero), battlefield, _battleGameplayManager);
            Heroes = new[] { Hero };
        }

        public IReadOnlyReactiveCollection<PlayerFighter> Units => _units;

        public HeroFighter Hero { get; }

        public IEnumerable<HeroFighter> Heroes { get; }

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;

        private readonly List<PlayerFighter> _unitsToDelete = new List<PlayerFighter>(32);

        void ITickable.Tick()
        {
            if (EnemySquad == null)
                return;

            foreach (var squadUnit in _units)
            {
                if (squadUnit.IsDead)
                {
                    _unitsToDelete.Add(squadUnit);
                    continue;
                }

                if (squadUnit.CurrentOrder is OrderAttack currentOrder)
                {
                    var toCurrentTarget = Vector2.Distance(
                        squadUnit.Position.Value.Vector,
                        currentOrder.Target.Position.Value.Vector);
                    if (toCurrentTarget < squadUnit.WeaponRange * BattleConstants.AIAggroResetMultiplier)
                        continue;
                }

                var order = _simpleAIService.AttackTheNearest(squadUnit, EnemySquad.Units.Where(u => !u.IsDead));
                if (order != null)
                    squadUnit.CurrentOrder = order;
            }
            
            foreach (var enemyUnit in _unitsToDelete)
                _units.Remove(enemyUnit);

            if (_units.Count < _battleGameplayManager.Parameters.PlayerSquadSize) 
                Spawn();
        }

        private void Spawn()
        {
            var parameters = _battleGameplayManager.Parameters;

            var point = new Vector2(
                Random.Range(-parameters.EnemySpawnSize, parameters.EnemySpawnSize),
                Random.Range(-parameters.EnemySpawnSize, parameters.EnemySpawnSize));

            var character = new Character(_battleGameplayManager.Parameters.TeammateDefaultMaxHp,
                _battleGameplayManager.Parameters.TeammateDefaultDamage);

            var playerUnit = new PlayerFighter(new Position(point), character, _battlefield);
            _units.Add(playerUnit);

            _battlefield.RegisterUnt(playerUnit);
        }

        void IDisposable.Dispose()
        {
            _agentsManager.Unregister(this);
        }
    }
}