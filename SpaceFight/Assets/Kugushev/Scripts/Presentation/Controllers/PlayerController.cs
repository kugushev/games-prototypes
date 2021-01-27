using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        // todo: add order controller and deal with orders in scriptable object. 
        // draw line and other staff based on CurrentOrder.CursorPosition
        
        public void HighlightPlanet(HandController sender, Planet planet)
        {
            planet.Selected = true;
        }

        public void HighlightPlanetCancel(HandController sender, Planet planet)
        {
            planet.Selected = false;
        }

        public void SelectArmy(HandController sender, Planet planet)
        {
            // OrderController.NewOrder
            // todo: start drawing line
        }

        public void SelectArmyCancel(HandController sender, Planet planet)
        {
            // OrderController.CancelOrder
            // todo: stop drawing line
        }
        
        // todo: add SelectArmyCommit

        public void MoveSelectedArmy(HandController sender, Planet planet, Vector3 position)
        {
            // OrderController.UpdateOrder
            // todo: fill Vector3 array and add next section of line
        }
    }
}