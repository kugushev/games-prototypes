using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Parameters;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.Models.Squad
{
    public class EnemySquad : BaseSquad, ITickable
    {
        private readonly PlayerSquad _playerSquad;
        private readonly SimpleAIService _simpleAIService;

        private readonly ReactiveCollection<EnemyUnit> _units = new ReactiveCollection<EnemyUnit>();

        public EnemySquad(BattleParameters battleParameters, PlayerSquad playerSquad,
            SimpleAIService simpleAIService, Battlefield battlefield)
        {
            _playerSquad = playerSquad;
            _simpleAIService = simpleAIService;
            for (var index = 0; index < battleParameters.Enemies.Count; index++)
            {
                var row = BattleConstants.UnitsPositionsInRow[index];
                var point = new Vector2(BattleConstants.EnemySquadLine, row);

                var enemyUnit = new EnemyUnit(new Position(point), battlefield);
                _units.Add(enemyUnit);

                battlefield.RegisterUnt(enemyUnit);
            }
        }

        public IReadOnlyReactiveCollection<EnemyUnit> Units => _units;

        protected override IReadOnlyList<BaseUnit> BaseUnits => _units;

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
    }
}