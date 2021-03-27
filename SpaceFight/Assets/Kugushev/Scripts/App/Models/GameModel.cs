using System;
using UnityEngine;

namespace Kugushev.Scripts.App.Models
{
    [Serializable]
    internal class GameModel
    {
        [SerializeField] private MainMenu mainMenu = new MainMenu();

        public MainMenu MainMenu => mainMenu; 
    }
}