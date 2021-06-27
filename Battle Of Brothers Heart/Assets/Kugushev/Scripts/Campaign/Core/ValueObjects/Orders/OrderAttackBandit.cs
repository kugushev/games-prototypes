using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core.ValueObjects.Orders
{
    public class OrderAttackBandit : OrderInteract<BanditWayfarer>
    {
        public class Factory : PlaceholderFactory<BanditWayfarer, OrderAttackBandit>
        {
        }
    }
}