using System.Collections.Generic;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects.Orders;

namespace Kugushev.Scripts.Core.Battle.Services
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
                var distanceVector = actor.Position.Value.Vector - opponent.Position.Value.Vector;
                if (minDistance > distanceVector.magnitude)
                {
                    minDistance = distanceVector.magnitude;
                    target = opponent;
                }
            }

            if (target != null)
                return _orderAttackFactory.Create(target);

            return null;
        }
    }
}