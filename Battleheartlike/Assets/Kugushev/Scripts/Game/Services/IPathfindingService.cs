using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Game.Services
{
    public interface IPathfindingService
    {
        bool TestDestination(in Position target);
    }
}