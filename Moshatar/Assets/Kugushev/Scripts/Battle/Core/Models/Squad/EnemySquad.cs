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
        private const int MaxSquadSize = 32; //12;
        private const int DefaultDamage = 1;
        private const int DefaultMaxHp = 6;
        public const float SpawnSize = 15f;

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

            // for (var index = 0; index < MaxSquadSize; index++)
            // {
            //     Spawn();
            // }
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

            int max = _director.GetMax();

            if (_units.Count(u => !u.IsDead) < max)
                Spawn();
        }

        private void Spawn()
        {
            var point = new Vector2(Random.Range(-SpawnSize, SpawnSize), Random.Range(-SpawnSize, SpawnSize));

            var character = new Character(DefaultMaxHp, DefaultDamage);

            var enemyUnit = new EnemyFighter(new Position(point), character, _battlefield);
            _units.Add(enemyUnit);

            _battlefield.RegisterUnt(enemyUnit);
        }

        void IDisposable.Dispose() => _agentsManager.Unregister(this);
    }
}