using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Models.Units;
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
        private readonly OrderAttack.Factory _orderAttackFactory;

        private readonly ReactiveCollection<EnemyUnit> _units = new ReactiveCollection<EnemyUnit>();

        public EnemySquad(BattleParameters battleParameters, PlayerSquad playerSquad,
            OrderAttack.Factory orderAttackFactory)
        {
            _playerSquad = playerSquad;
            _orderAttackFactory = orderAttackFactory;

            foreach (var _ in battleParameters.Enemies)
            {
                _units.Add(new EnemyUnit());
            }
        }

        public IReadOnlyReactiveCollection<EnemyUnit> Units => _units;

        protected override IReadOnlyList<BaseUnit> BaseUnits => _units;

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units)
            {
                if (enemyUnit.CurrentOrder is null)
                {
                    var target = _playerSquad.Units[Random.Range(0, _units.Count)];
                    enemyUnit.CurrentOrder = _orderAttackFactory.Create(target);
                }
            }
        }
    }
}