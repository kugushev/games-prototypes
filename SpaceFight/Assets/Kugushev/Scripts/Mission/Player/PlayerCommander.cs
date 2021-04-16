using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Player
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Player Commander")]
    public class PlayerCommander : ScriptableObject, ICommander
    {
        [SerializeField] private OrdersManager? leftHandOrders;
        [SerializeField] private OrdersManager? rightHandOrders;

        public bool Surrendered
        {
            get
            {
                Asserting.NotNull(leftHandOrders, rightHandOrders);
                return leftHandOrders.Surrendered || rightHandOrders.Surrendered;
            }
        }

        public void AssignFleet(Fleet fleet, Faction faction)
        {
            Asserting.NotNull(leftHandOrders, rightHandOrders);
            leftHandOrders.AssignFleet(fleet, faction);
            rightHandOrders.AssignFleet(fleet, faction);
        }

        public void WithdrawFleet()
        {
            Asserting.NotNull(leftHandOrders, rightHandOrders);
            leftHandOrders.WithdrawFleet();
            rightHandOrders.WithdrawFleet();
        }
    }
}