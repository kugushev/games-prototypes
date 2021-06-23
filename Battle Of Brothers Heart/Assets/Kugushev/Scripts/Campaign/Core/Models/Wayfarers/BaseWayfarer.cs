using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public abstract class BaseWayfarer : ActiveAgent, IInteractable
    {
        protected BaseWayfarer(Position position) : base(position)
        {
        }

        #region IInteractable

        Position IInteractable.Position => Position.Value;
        bool IInteractable.IsInteractable => IsActive;

        #endregion

        protected override bool IsActive => true;
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