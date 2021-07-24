using Kugushev.Scripts.City.Interfaces;

namespace Kugushev.Scripts.City.Components.Commands
{
    public readonly struct InteractCommand
    {
        public InteractCommand(IInteractable interactable) => Interactable = interactable;

        public readonly IInteractable Interactable;
    }
}