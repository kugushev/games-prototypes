using System.Collections.Generic;

namespace Kugushev.Scripts.Common.Core.AI
{
    public interface IAgentsOwner
    {
        IEnumerable<IAgent> Agents { get; }
    }
}