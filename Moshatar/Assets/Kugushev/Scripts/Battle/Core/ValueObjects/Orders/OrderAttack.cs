using Kugushev.Scripts.Battle.Core.AI.Orders;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.ValueObjects.Orders
{
    public class OrderAttack : OrderInteract<BaseFighter>
    {
        public class Factory : PlaceholderFactory<BaseFighter, OrderAttack>
        {
        }
    }
}