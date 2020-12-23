using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Game.Services
{
    public interface INavigationService
    {
        bool TrySetDestination(in Position target);
        bool TestIfDestinationReached();
    }
}