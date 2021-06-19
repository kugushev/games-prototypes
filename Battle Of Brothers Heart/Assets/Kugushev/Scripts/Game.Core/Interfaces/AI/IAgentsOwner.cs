using System.Collections.Generic;

namespace Kugushev.Scripts.Game.Core.Interfaces.AI
{
    public interface IAgentsOwner
    {
        IEnumerable<IAgent> Agents { get; }
    }
}