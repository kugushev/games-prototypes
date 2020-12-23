using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Game.Services
{
    public interface INavigationComponent
    {
        bool TrySetDestination(in Position target);
        bool TestIfDestinationReached();
    }
}