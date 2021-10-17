using System.Collections.Generic;

namespace Kugushev.Scripts.Battle.Core.AI
{
    public interface IAgentsOwner
    {
        IEnumerable<IAgent> Agents { get; }
    }
}