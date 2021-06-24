using Kugushev.Scripts.City.Core.Models;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Zenject;

namespace Kugushev.Scripts.City.Core.ValueObjects.Orders
{
    public class OrderGoToRoadSign : OrderInteract<RoadSign>
    {
        public class Factory : PlaceholderFactory<RoadSign, OrderGoToRoadSign>
        {
        }
    }
}