using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Player
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Player Commander")]
    public class PlayerCommander : ScriptableObject, ICommander
    {
        [SerializeField] private OrdersManager leftHandOrders;
        [SerializeField] private OrdersManager rightHandOrders;

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