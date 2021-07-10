using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Models.WorldUnits;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class PlayerWayfarer : BaseWayfarer
    {
        private readonly GameModeManager _gameModeManager;
        private readonly BattleManager _battleManager;

        public PlayerWayfarer(WorldUnit worldUnit, GameModeManager gameModeManager, BattleManager battleManager)
            : base(worldUnit)
        {
            WorldUnit = worldUnit;
            _gameModeManager = gameModeManager;
            _battleManager = battleManager;
        }


        protected override float Speed => CampaignConstants.Wayfarers.PlayerSpeed;

        public override WorldUnit WorldUnit { get; }

        protected override OrderProcessingStatus ProcessInteraction(OrderInteract order)
        {
            switch (order)
            {
                case OrderVisitCity visitCity:
                    Debug.Log($"City {visitCity.Target.Name} visited");

                    // todo: think how to run it with await
                    _gameModeManager.SwitchToCityModeAsync();

                    return OrderProcessingStatus.Finished;
                case OrderAttackBandit attackBandit:

                    Debug.Log("Attack bandit");
                    _battleManager.StartBattleAsync(attackBandit.Target.BanditWorldUnit);

                    break;
                default:
                    Debug.LogError($"Unexpected order {order}");
                    break;
            }

            return OrderProcessingStatus.InProgress;
        }
    }
}