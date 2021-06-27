using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class BanditWayfarer : BaseWayfarer
    {
        public BanditWayfarer(WorldUnit worldUnit) : base(worldUnit)
        {
        }

        protected override OrderProcessingStatus ProcessInteraction(OrderInteract order)
        {
            return OrderProcessingStatus.InProgress;
        }
    }
}