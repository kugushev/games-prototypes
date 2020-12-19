using Kugushev.Scripts.Core.Activities.Abstractions;
using Kugushev.Scripts.Core.Providers;

namespace Kugushev.Scripts.Core.Behaviors
{
    public interface IMovable: IInteractable
    {
        IPathfindingProvider PathfindingProvider { get; }
    }
}