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
        private const int MaxSquadSize = 22;
        private const int DefaultDamage = 1;
        private const int DefaultMaxHp = 4;
        private const int BigDamage = 4;
        private const int BigMaxHp = 40;
        public const float SpawnSize = 8f;

        private readonly PlayerSquad _playerSquad;
        private readonly SimpleAIService _simpleAIService;
        private readonly Battlefield _battlefield;
        private readonly AgentsManager _agentsManager;
        private readonly Director _director;

        private readonly ReactiveCollection<EnemyFighter> _units = new ReactiveCollection<EnemyFighter>();

        public EnemySquad(PlayerSquad playerSquad, SimpleAIService simpleAIService, Battlefield battlefield,
            AgentsManager agentsManager, Director director)
        {
            _playerSquad = playerSquad;
            _playerSquad.EnemySquad = this;

            _simpleAIService = simpleAIService;
            _battlefield = battlefield;
            _agentsManager = agentsManager;
            _director = director;

            _agentsManager.Register(this);
        }

        public IReadOnlyReactiveCollection<EnemyFighter> Units => _units;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;


        private readonly List<EnemyFighter> _unitsToDelete = new List<EnemyFighter>(MaxSquadSize);

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
            var point = new Vector2(Random.Range(-SpawnSize, SpawnSize), Random.Range(-SpawnSize, SpawnSize));

            var character = spawnBig
                ? new Character(BigMaxHp, BigDamage)
                : new Character(DefaultMaxHp, DefaultDamage);

            var enemyUnit = new EnemyFighter(new Position(point), character, _battlefield, spawnBig);
            _units.Add(enemyUnit);

            _battlefield.RegisterUnt(enemyUnit);
        }

        void IDisposable.Dispose() => _agentsManager.Unregister(this);
    }
}