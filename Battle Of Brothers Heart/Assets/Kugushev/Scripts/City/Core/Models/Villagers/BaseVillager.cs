using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.City.Core.Models.Villagers
{
    public abstract class BaseVillager : ActiveAgent
    {
        protected BaseVillager(Position battlefieldPosition) : base(battlefieldPosition)
        {
        }

        protected override bool IsActive => true;
        protected override float InteractionRadius => CityConstants.Villagers.InteractionRange;
        protected override float Speed => CityConstants.Villagers.Speed;

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