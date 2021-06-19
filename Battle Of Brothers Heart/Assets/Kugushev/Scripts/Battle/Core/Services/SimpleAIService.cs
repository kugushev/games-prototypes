using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Core.ValueObjects.Orders;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Services
{
    public class SimpleAIService
    {
        private readonly OrderAttack.Factory _orderAttackFactory;

        public SimpleAIService(OrderAttack.Factory orderAttackFactory)
        {
            _orderAttackFactory = orderAttackFactory;
        }

        public OrderAttack? AttackTheNearest(BaseUnit actor, IEnumerable<BaseUnit> opponents)
        {
            float minDistance = float.MaxValue;
            BaseUnit? target = null;
            foreach (var opponent in opponents)
            {
                var distance = Vector2.Distance(actor.Position.Value.Vector, opponent.Position.Value.Vector);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    target = opponent;
                }
            }

            if (target != null)
                return _orderAttackFactory.Create(target);

            return null;
        }
    }
}