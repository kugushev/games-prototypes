using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Models.WorldUnits;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class BanditWayfarer : BaseWayfarer
    {
        private readonly BattleManager _battleManager;

        public BanditWayfarer(BanditWorldUnit worldUnit, BattleManager battleManager) : base(worldUnit)
        {
            BanditWorldUnit = worldUnit;
            _battleManager = battleManager;
            City = worldUnit.HomeCity;
        }

        public City City { get; }

        public BanditWorldUnit BanditWorldUnit { get; }
        public override WorldUnit WorldUnit => BanditWorldUnit;

        protected override float Speed => CampaignConstants.Wayfarers.Speed;

        protected override OrderProcessingStatus ProcessInteraction(OrderInteract order)
        {
            switch (order)
            {
                case OrderAttackPlayer _:
                    _battleManager.StartBattleAsync(BanditWorldUnit);
                    break;
                default:
                    Debug.LogError($"Unexpected order {order} for bandit {this}");
                    break;
            }

            return OrderProcessingStatus.InProgress;
        }
    }
}