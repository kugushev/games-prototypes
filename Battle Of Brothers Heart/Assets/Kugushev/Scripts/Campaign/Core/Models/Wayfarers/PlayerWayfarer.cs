using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class PlayerWayfarer : BaseWayfarer
    {
        public PlayerWayfarer(WorldUnit worldUnit) : base(worldUnit)
        {
        }

        protected override OrderProcessingStatus ProcessInteraction(OrderInteract order)
        {
            switch (order)
            {
                case OrderVisitCity visitCity:
                    Debug.Log($"City {visitCity.Target.Name} visited");
                    return OrderProcessingStatus.Finished;
                default:
                    Debug.LogError($"Unexpected order {order}");
                    break;
            }

            return OrderProcessingStatus.InProgress;
        }
    }
}