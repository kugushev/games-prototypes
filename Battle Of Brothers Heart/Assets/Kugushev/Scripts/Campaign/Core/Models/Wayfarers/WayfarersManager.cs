using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.Controllers;
using Kugushev.Scripts.Game.Core.Interfaces.AI;
using Kugushev.Scripts.Game.Core.Models.AI;
using Kugushev.Scripts.Game.Core.Models.AI.Orders;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class WayfarersManager : IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;

        public WayfarersManager(InputController inputController, OrderMove.Factory orderMoveFactory,
            AgentsManager agentsManager)
        {
            _agentsManager = agentsManager;
            _agentsManager.Register(this);
            Player = new PlayerWayfarer(new Position(new Vector2(0, 0)), inputController, orderMoveFactory);
            Agents = new[] {Player};
        }

        public PlayerWayfarer Player { get; }
        public IEnumerable<IAgent> Agents { get; }

        void IDisposable.Dispose()
        {
            _agentsManager.Unregister(this);
        }
    }
}