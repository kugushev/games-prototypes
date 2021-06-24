using System;
using System.Collections.Generic;
using Kugushev.Scripts.City.Core.ValueObjects.Orders;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.City.Core.Models.Villagers
{
    public class PlayerVillager : BaseVillager, IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;


        public PlayerVillager(AgentsManager agentsManager) : base(new Position(new Vector2(-2f, -2f)))
        {
            Agents = new[] {this};

            _agentsManager = agentsManager;
            _agentsManager.Register(this);
        }

        protected override OrderProcessingStatus ProcessInteraction(OrderInteract order)
        {
            switch (order)
            {
                case OrderGoToRoadSign goToReadSign:
                    Debug.Log($"Go out");
                    break;
                default:
                    Debug.LogError($"Unexpected order {order}");
                    break;
            }

            return OrderProcessingStatus.InProgress;
        }

        public IEnumerable<IAgent> Agents { get; }

        public void Dispose() => _agentsManager.Unregister(this);
    }
}