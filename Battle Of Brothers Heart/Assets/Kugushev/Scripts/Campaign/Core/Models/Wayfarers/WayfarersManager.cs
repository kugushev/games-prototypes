using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Game.Core.Managers;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class WayfarersManager : IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;

        public WayfarersManager(AgentsManager agentsManager, WorldUnitsManager worldUnitsManager,
            GameModeManager gameModeManager, BattleManager battleManager)
        {
            _agentsManager = agentsManager;
            _agentsManager.Register(this);

            Player = new PlayerWayfarer(worldUnitsManager.Player, gameModeManager, battleManager);
            Bandits = worldUnitsManager.Bandits.Select(u => new BanditWayfarer(u)).ToArray();

            Agents = new[] {Player};
        }

        public PlayerWayfarer Player { get; }

        public IReadOnlyList<BanditWayfarer> Bandits { get; }
        public IEnumerable<IAgent> Agents { get; }

        void IDisposable.Dispose()
        {
            _agentsManager.Unregister(this);
        }
    }
}