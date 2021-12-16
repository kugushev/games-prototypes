﻿using System;
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
        private readonly OrderAttack.Factory _orderAttackFactory;

        private readonly ReactiveCollection<EnemyFighter> _units = new ReactiveCollection<EnemyFighter>();

        public EnemySquad(PlayerSquad playerSquad, SimpleAIService simpleAIService, Battlefield battlefield,
            AgentsManager agentsManager, Director director, BattleGameplayManager battleGameplayManager,
            OrderAttack.Factory orderAttackFactory)
        {
            _playerSquad = playerSquad;
            _playerSquad.EnemySquad = this;

            _simpleAIService = simpleAIService;
            _battlefield = battlefield;
            _agentsManager = agentsManager;
            _director = director;
            _battleGameplayManager = battleGameplayManager;
            _orderAttackFactory = orderAttackFactory;

            _agentsManager.Register(this);
        }

        public IReadOnlyReactiveCollection<EnemyFighter> Units => _units;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;


        private readonly List<EnemyFighter> _unitsToDelete = new List<EnemyFighter>(32);

        public IReadOnlyList<Vector3> SpawnPoints { get; set; }
        private int _lastSpawnIndex = -1;

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units)
            {
                if (enemyUnit.IsDead)
                {
                    _unitsToDelete.Add(enemyUnit);
                    continue;
                }

                ApplyOrders(enemyUnit);
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

        private void ApplyOrders(EnemyFighter enemyUnit)
        {
            if (enemyUnit.CurrentOrder is OrderAttack currentOrder)
            {
                var toCurrentTarget = Vector2.Distance(
                    enemyUnit.Position.Value.Vector,
                    currentOrder.Target.Position.Value.Vector);
                if (toCurrentTarget < enemyUnit.WeaponRange * BattleConstants.AIAggroResetMultiplier)
                    return;
            }

            if (enemyUnit.IsHeroHunter)
            {
                enemyUnit.CurrentOrder = _orderAttackFactory.Create(_playerSquad.Hero);
                return;
            }

            if (enemyUnit.IsConqueror)
            {
                var point = _playerSquad.DefendingPoints.FirstOrDefault(p => !p.IsDead);
                if (point is { })
                {
                    enemyUnit.CurrentOrder = _orderAttackFactory.Create(point);
                    return;
                }
            }

            // todo: this might be slow operation, let's measure impact
            var opponents = _playerSquad.Units.Where(u => !u.IsDead)
                .Concat<BaseFighter>(_playerSquad.Heroes)
                .Concat(_playerSquad.DefendingPoints.Where(u => !u.IsDead));

            var order = _simpleAIService.AttackTheNearest(enemyUnit, opponents);
            if (order != null)
                enemyUnit.CurrentOrder = order;
        }

        private void Spawn(bool isBig)
        {
            var parameters = _battleGameplayManager.Parameters;

            Vector2 point;
            if (_battleGameplayManager.SeletedMode == BattleGameplayManager.Mode.Tog)
            {
                if (SpawnPoints == null)
                    return;

                point = GetSpawnPoint();
            }
            else
                point = new Vector2(
                    Random.Range(-parameters.EnemySpawnSize, parameters.EnemySpawnSize),
                    Random.Range(-parameters.EnemySpawnSize, parameters.EnemySpawnSize));

            var character = isBig
                ? new Character(parameters.EnemyBigMaxHp, parameters.EnemyBigDamage, parameters.EnemyBigAttackRange)
                : new Character(parameters.EnemyDefaultMaxHp, parameters.EnemyDefaultDamage);

            var isHeroHunter = !isBig && Random.Range(0f, 1f) < parameters.EnemyHeroHunterSpawnProbability;
            var isConqueror = !isBig && !isHeroHunter &&
                              Random.Range(0f, 1f) < parameters.EnemyConquerorSpawnProbability;

            var enemyUnit = new EnemyFighter(new Position(point), character, _battlefield,
                isBig,
                isHeroHunter,
                isConqueror);
            _units.Add(enemyUnit);

            _battlefield.RegisterUnt(enemyUnit);
        }

        private Vector2 GetSpawnPoint()
        {
            var spawnIndex = _lastSpawnIndex + 1;
            if (spawnIndex >= SpawnPoints.Count)
                spawnIndex = 0;

            var vector = SpawnPoints[spawnIndex];
            var point = new Vector2(vector.x, vector.z);

            _lastSpawnIndex = spawnIndex;
            return point;
        }

        // private Vector2 GetSpawnPoint()
        // {
        //     int spawnIndex;
        //     do
        //     {
        //         spawnIndex = Random.Range(0, SpawnPoints.Count);
        //     } while (spawnIndex == _lastSpawnIndex);
        //
        //     _lastSpawnIndex = spawnIndex;
        //
        //     var vector = SpawnPoints[spawnIndex];
        //     var point = new Vector2(vector.x, vector.z);
        //     return point;
        // }

        void IDisposable.Dispose() => _agentsManager.Unregister(this);
    }
}