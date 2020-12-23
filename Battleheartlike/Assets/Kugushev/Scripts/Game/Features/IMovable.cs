using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Services;

namespace Kugushev.Scripts.Game.Features
{
    public interface IMovable
    {
        INavigationComponent NavigationComponent { get; }
        
        bool IsMoving { get; set; }
    }
}