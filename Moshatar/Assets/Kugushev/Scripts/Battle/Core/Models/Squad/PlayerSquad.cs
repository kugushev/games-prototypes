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
        private readonly ReactiveProperty<int> _availableUnits = new ReactiveProperty<int>(100);
        private readonly List<PlayerFighter> _unitsToDelete = new List<PlayerFighter>(32);
        private readonly List<DefendingPoint> _defendingPoints;
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

            Hero = new HeroFighter(new Position(Vector2.zero), battlefield, _battleGameplayManager);
            Heroes = new[] { Hero };
            _defendingPoints = _battleGameplayManager.Parameters.DefendingPoints
                .Select(CreateDefendingPoint)
                .ToList();
        }

        public IReadOnlyReactiveCollection<PlayerFighter> Units => _units;

        public HeroFighter Hero { get; }

        public IEnumerable<HeroFighter> Heroes { get; }

        public IReadOnlyList<DefendingPoint> DefendingPoints => _defendingPoints;

        public IReadOnlyReactiveProperty<int> AvailableUnits => _availableUnits;

        public IReadOnlyList<Vector3> SpawnPoints { get; set; }
        private int _lastSpawnIndex = 0;

        public event Action GameOver;

        IEnumerable<IAgent> IAgentsOwner.Agents => _units;

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

            if (DefendingPoints.All(d => d.IsDead)) 
                GameOver?.Invoke();
        }

        private void Spawn()
        {
            if (_availableUnits.Value <= 0)
                return;

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


            var character = new Character(parameters.TeammateDefaultMaxHp,
                parameters.TeammateDefaultDamage);

            var playerUnit = new PlayerFighter(new Position(point), character, _battlefield);
            _units.Add(playerUnit);

            _battlefield.RegisterUnt(playerUnit);

            _availableUnits.Value--;
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

        private DefendingPoint CreateDefendingPoint(Vector2 v) => new DefendingPoint(
            new Position(v),
            _battleGameplayManager.Parameters.DefendingPointHealth,
            _battlefield);

        void IDisposable.Dispose()
        {
            _agentsManager.Unregister(this);
        }
    }
}