using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Game.Core.Models.AI.Orders;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Orders
{
    public class OrderAttack : OrderInteract<BaseUnit>
    {
        public class Factory : PlaceholderFactory<BaseUnit, OrderAttack>
        {
        }
    }
}