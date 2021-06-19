using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Interfaces.AI;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Core.Models.AI
{
    public class AgentsManager : IFixedTickable
    {
        private readonly List<IAgentsOwner> _agentsOwners = new List<IAgentsOwner>(32);

        public void Register(IAgentsOwner agentsOwner) => _agentsOwners.Add(agentsOwner);
        public void Unregister(IAgentsOwner agentsOwner) => _agentsOwners.Remove(agentsOwner);

        public void FixedTick()
        {
            foreach (var agentOwner in _agentsOwners)
            foreach (var agent in agentOwner.Agents)
            {
                agent.ProcessCurrentOrder(new DeltaTime(Time.fixedDeltaTime));
            }
        }
    }
}