using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Core.Battle.Models
{
    public class EnemySquad : ITickable
    {
        private readonly Squad _squad;
        private readonly OrderAttack.Factory _orderAttackFactory;
        private readonly List<EnemyUnit> _units = new List<EnemyUnit>();


        public EnemySquad(Squad squad, OrderAttack.Factory orderAttackFactory)
        {
            _squad = squad;
            _orderAttackFactory = orderAttackFactory;
        }

        public void Add(EnemyUnit unit) => _units.Add(unit);

        void ITickable.Tick()
        {
            foreach (var enemyUnit in _units)
            {
                if (enemyUnit.CurrentOrder is null)
                {
                    var target = _squad.Units[Random.Range(0, _units.Count)];
                    enemyUnit.CurrentOrder = _orderAttackFactory.Create(target);
                }
            }
        }
        
        public void ProcessOrders(DeltaTime delta)
        {
            foreach (var unit in _units)
            {
                unit.ProcessCurrentOrder(delta);
            }
        }
    }
}