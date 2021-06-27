using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class PlayerWayfarer : BaseWayfarer
    {
        private readonly GameModeManager _gameModeManager;

        public PlayerWayfarer(WorldUnit worldUnit, GameModeManager gameModeManager) : base(worldUnit)
        {
            _gameModeManager = gameModeManager;
        }

        protected override OrderProcessingStatus ProcessInteraction(OrderInteract order)
        {
            switch (order)
            {
                case OrderVisitCity visitCity:
                    Debug.Log($"City {visitCity.Target.Name} visited");

                    // todo: think how to run it with await
                    _gameModeManager.SwitchToCityModeAsync();

                    return OrderProcessingStatus.Finished;
                default:
                    Debug.LogError($"Unexpected order {order}");
                    break;
            }

            return OrderProcessingStatus.InProgress;
        }
    }
}