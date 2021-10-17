using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Interfaces
{
    public interface IInteractable
    {
        Position Position { get; }
        bool IsInteractable { get; }
    }
}