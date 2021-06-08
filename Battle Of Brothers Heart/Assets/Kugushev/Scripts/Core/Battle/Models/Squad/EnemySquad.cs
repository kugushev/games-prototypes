using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Core.Battle.Models.Squad
{
    public class EnemySquad : BaseSquad<EnemyUnit>, ITickable
    {
        private readonly PlayerSquad _playerSquad;
        private readonly OrderAttack.Factory _orderAttackFactory;
        private readonly List<EnemyUnit> _units = new List<EnemyUnit>();


        public EnemySquad(PlayerSquad playerSquad, OrderAttack.Factory orderAttackFactory)
        {
            _playerSquad = playerSquad;
            _orderAttackFactory = orderAttackFactory;
        }

        public void Add(EnemyUnit unit) => _units.Add(unit);

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units)
            {
                if (enemyUnit.CurrentOrder is null)
                {
                    var target = _playerSquad.BaseUnits[Random.Range(0, _units.Count)];
                    enemyUnit.CurrentOrder = _orderAttackFactory.Create(target);
                }
            }
        }
    }
}