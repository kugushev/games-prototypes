using System;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public abstract class BaseWayfarer : ActiveAgent, IInteractable
    {
        protected BaseWayfarer(WorldUnit worldUnit)
            : base(worldUnit.Position)
        {
            WorldUnit = worldUnit;
            worldUnit.SubscribeTo(Position);
            worldUnit.SubscribeTo(Direction);
        }

        #region IInteractable

        Position IInteractable.Position => Position.Value;
        bool IInteractable.IsInteractable => true;

        #endregion

        public WorldUnit WorldUnit { get; }
        protected override bool IsActive => !WorldUnit.IsFrozen;
        protected override float InteractionRadius => CampaignConstants.Wayfarers.InteractionRange;
        protected override float Speed => CampaignConstants.Wayfarers.Speed;

        protected override bool CanProcessOrder()
        {
            // todo: return false if modal menu is enabled
            return true;
        }

        protected override void CancelInteraction()
        {
        }

        protected override bool CheckCollisions(Vector2 movement, float movementDistance, Vector2 target)
        {
            return true;
        }
    }
}