using Kugushev.Scripts.Models.Services;
using Kugushev.Scripts.ValueObjects;

namespace Kugushev.Scripts.Models.Behaviors
{
    public interface IMovable
    {
        IPathfindingService PathfindingService { get; }
        
        Position? Destination { get; set; }
    }
}