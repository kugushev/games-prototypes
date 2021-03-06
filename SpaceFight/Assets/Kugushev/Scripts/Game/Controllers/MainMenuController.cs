using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Controllers
{
    public class MainMenuController: MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;

        public void StartGame()
        {
            mainMenu.StartClicked = true;
        }
    }
}