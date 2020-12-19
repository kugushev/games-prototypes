using Kugushev.Scripts.Core.ValueObjects;

namespace Kugushev.Scripts.Core.Providers
{
    public interface IPathfindingProvider
    {
        bool TestDestination(Position target);
    }
}