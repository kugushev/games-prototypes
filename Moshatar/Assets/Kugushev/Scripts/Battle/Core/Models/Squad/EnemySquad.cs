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
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class EnemySquad : ITickable, IDisposable, IAgentsOwner
    {
        private readonly PlayerSquad _playerSquad;
        private readonly SimpleAIService _simpleAIService;
        private readonly Battlefield _battlefield;
        private readonly AgentsManager _agentsManager;
        private readonly Director _director;
        private readonly BattleGameplayManager _battleGameplayManager;

        private readonly ReactiveCollection<EnemyFighter> _units = new ReactiveCollection<EnemyFighter>();

        public EnemySquad(PlayerSquad playerSquad, SimpleAIService simpleAIService, Battlefield battlefield,
            AgentsManager agentsManager, Director director, BattleGameplayManager battleGameplayManager)
        {
            _playerSquad = playerSquad;
            _playerSquad.EnemySquad = this;

            _simpleAIService = simpleAIService;
            _battlefield = battlefield;
            _agentsManager = agentsManager;
            _director = director;
            _battleGameplayManager = battleGameplayManager;

            _agentsManager.Register(this);
        }

        public IReadOnlyReactiveCollection<EnemyFighter> Units => _units;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;


        private readonly List<EnemyFighter> _unitsToDelete = new List<EnemyFighter>(32);

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units)
            {
                if (enemyUnit.IsDead)
                {
                    _unitsToDelete.Add(enemyUnit);
                    continue;
                }

                // apply orders
                if (enemyUnit.CurrentOrder is OrderAttack currentOrder)
                {
                    var toCurrentTarget = Vector2.Distance(
                        enemyUnit.Position.Value.Vector,
                        currentOrder.Target.Position.Value.Vector);
                    if (toCurrentTarget < enemyUnit.WeaponRange * BattleConstants.AIAggroResetMultiplier)
                        continue;
                }

                var opponents = _playerSquad.Units.Where(u => !u.IsDead).Concat<BaseFighter>(_playerSquad.Heroes);
                var order = _simpleAIService.AttackTheNearest(enemyUnit, opponents);
                if (order != null)
                    enemyUnit.CurrentOrder = order;
            }

            foreach (var enemyUnit in _unitsToDelete)
                _units.Remove(enemyUnit);


            var (max, spawnBig, idx) = _director.GetMax();
            if (_units.Count < max)
            {
                if (spawnBig)
                    _director.RegisterBigSpawning(idx);

                Spawn(spawnBig);
            }
        }

        private void Spawn(bool spawnBig)
        {
            var parameters = _battleGameplayManager.Parameters;

            var point = new Vector2(
                Random.Range(-parameters.EnemySpawnSize, parameters.EnemySpawnSize),
                Random.Range(-parameters.EnemySpawnSize, parameters.EnemySpawnSize));

            var character = spawnBig
                ? new Character(parameters.EnemyBigMaxHp, parameters.EnemyBigDamage, parameters.EnemyBigAttackRange)
                : new Character(parameters.EnemyDefaultMaxHp, parameters.EnemyDefaultDamage);

            var enemyUnit = new EnemyFighter(new Position(point), character, _battlefield, spawnBig);
            _units.Add(enemyUnit);

            _battlefield.RegisterUnt(enemyUnit);
        }

        void IDisposable.Dispose() => _agentsManager.Unregister(this);
    }
}