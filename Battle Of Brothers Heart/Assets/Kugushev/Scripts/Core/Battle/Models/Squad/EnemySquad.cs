using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.Services;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using Kugushev.Scripts.Core.Game.Parameters;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Core.Battle.Models.Squad
{
    public class EnemySquad : BaseSquad, ITickable
    {
        private readonly PlayerSquad _playerSquad;
        private readonly SimpleAIService _simpleAIService;

        private readonly ReactiveCollection<EnemyUnit> _units = new ReactiveCollection<EnemyUnit>();

        public EnemySquad(BattleParameters battleParameters, PlayerSquad playerSquad,
            SimpleAIService simpleAIService)
        {
            _playerSquad = playerSquad;
            _simpleAIService = simpleAIService;
            for (var index = 0; index < battleParameters.Enemies.Count; index++)
            {
                var row = BattleConstants.UnitsPositionsInRow[index];
                var point = new Vector2(BattleConstants.EnemySquadLine, row);
                _units.Add(new EnemyUnit(new Position(point)));
            }
        }

        public IReadOnlyReactiveCollection<EnemyUnit> Units => _units;

        protected override IReadOnlyList<BaseUnit> BaseUnits => _units;

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units)
            {
                if (enemyUnit.CurrentOrder is OrderAttack currentOrder)
                {
                    var toCurrentTarget = enemyUnit.Position.Value.Vector - currentOrder.Target.Position.Value.Vector;
                    if (toCurrentTarget.magnitude < enemyUnit.WeaponRange * BattleConstants.AIAggroResetMultiplier)
                        continue;
                }

                var order = _simpleAIService.AttackTheNearest(enemyUnit, _playerSquad.Units);
                if (order != null)
                    enemyUnit.CurrentOrder = order;
            }
        }
    }
}