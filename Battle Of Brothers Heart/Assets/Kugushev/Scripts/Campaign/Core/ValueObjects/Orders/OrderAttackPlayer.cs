using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core.ValueObjects.Orders
{
    public class OrderAttackPlayer : OrderInteract<PlayerWayfarer>
    {
        public class Factory : PlaceholderFactory<PlayerWayfarer, OrderAttackPlayer>
        {
        }
    }
}