using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Game.Core.Managers;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class WayfarersManager : IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;

        public WayfarersManager(AgentsManager agentsManager, UnitsManager unitsManager)
        {
            _agentsManager = agentsManager;
            _agentsManager.Register(this);

            Player = new PlayerWayfarer(unitsManager.Player);
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