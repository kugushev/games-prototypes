using Kugushev.Scripts.City.Interfaces;

namespace Kugushev.Scripts.City.Models.Cells
{
    public class InteractableCell: CityCell
    {
        public InteractableCell(IInteractable interactable) => Interactable = interactable;

        public IInteractable Interactable { get; }
    }
}