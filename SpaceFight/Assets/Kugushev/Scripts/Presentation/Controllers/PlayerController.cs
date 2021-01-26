using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class PlayerController: MonoBehaviour
    {
        public void SelectPlanet(Planet planet)
        {
            planet.Selected = true;
            print("Selected");
        }

        public void DeselectPlanet(Planet planet)
        {
            planet.Selected = false;
            print("Deselected");
        }
        
    }
}