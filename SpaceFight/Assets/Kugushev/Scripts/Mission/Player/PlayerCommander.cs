using Kugushev.Scripts.Common;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Player
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Player Commander")]
    public class PlayerCommander : ScriptableObject, ICommander
    {
        [SerializeField] private OrdersManager leftHandOrders;
        [SerializeField] private OrdersManager rightHandOrders;

        public bool Surrendered => leftHandOrders.Surrendered || rightHandOrders.Surrendered;

        public void AssignFleet(Fleet fleet, Faction faction)
        {
            leftHandOrders.AssignFleet(fleet, faction);
            rightHandOrders.AssignFleet(fleet, faction);
        }

        public void WithdrawFleet()
        {
            leftHandOrders.WithdrawFleet();
            rightHandOrders.WithdrawFleet();
        }
    }
}