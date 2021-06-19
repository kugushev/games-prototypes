using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Interfaces
{
    public interface IInteractable
    {
        Position Position { get; }
        bool IsInteractable { get; }
    }
}