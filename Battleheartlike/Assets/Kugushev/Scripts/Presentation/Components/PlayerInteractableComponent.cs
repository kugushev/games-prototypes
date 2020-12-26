using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using Kugushev.Scripts.Presentation.Components.Abstractions;

namespace Kugushev.Scripts.Presentation.Components
{
    public class PlayerInteractableComponent : BaseComponent<Character> 
    {
        public Character Character => Model;
    }
}