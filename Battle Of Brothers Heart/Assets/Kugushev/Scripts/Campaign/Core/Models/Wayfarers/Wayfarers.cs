using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Game.Core.Managers;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class Wayfarers : IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;

        public Wayfarers(AgentsManager agentsManager, WorldUnitsManager worldUnitsManager,
            GameModeManager gameModeManager, BattleManager battleManager)
        {
            _agentsManager = agentsManager;
            _agentsManager.Register(this);

            Player = new PlayerWayfarer(worldUnitsManager.Player, gameModeManager, battleManager);
            Bandits = worldUnitsManager.Units.Select(u => new BanditWayfarer(u)).ToArray();
        }

        public PlayerWayfarer Player { get; }

        public IReadOnlyList<BanditWayfarer> Bandits { get; }

        IEnumerable<IAgent> IAgentsOwner.Agents
        {
            get
            {
                yield return Player;
                foreach (var bandit in Bandits)
                {
                    yield return bandit;
                }
            }
        }

        void IDisposable.Dispose()
        {
            _agentsManager.Unregister(this);
        }
    }
}