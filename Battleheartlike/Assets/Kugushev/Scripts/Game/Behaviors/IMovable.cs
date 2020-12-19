using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Services;

namespace Kugushev.Scripts.Game.Behaviors
{
    public interface IMovable
    {
        IPathfindingService PathfindingService { get; }
        
        Position? Destination { get; set; }
    }
}