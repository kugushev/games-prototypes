using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Game.Core.Managers;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class WayfarersManager : IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;

        public WayfarersManager(AgentsManager agentsManager, WorldUnitsManager worldUnitsManager,
            GameModeManager gameModeManager)
        {
            _agentsManager = agentsManager;
            _agentsManager.Register(this);

            Player = new PlayerWayfarer(worldUnitsManager.Player, gameModeManager);
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