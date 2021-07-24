using Kugushev.Scripts.City.Interfaces;

namespace Kugushev.Scripts.City.Components
{
    public readonly struct HeroInteractCommand
    {
        public HeroInteractCommand(IInteractable interactable) => Interactable = interactable;

        public IInteractable Interactable { get; }
    }
}