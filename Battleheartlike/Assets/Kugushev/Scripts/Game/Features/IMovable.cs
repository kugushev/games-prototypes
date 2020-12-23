using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Services;

namespace Kugushev.Scripts.Game.Features
{
    public interface IMovable
    {
        INavigationService NavigationService { get; }
        
        bool IsMoving { get; set; }
    }
}