using Kugushev.Scripts.ValueObjects;

namespace Kugushev.Scripts.Models.Services
{
    public interface IPathfindingService
    {
        bool TestDestination(in Position target);
    }
}