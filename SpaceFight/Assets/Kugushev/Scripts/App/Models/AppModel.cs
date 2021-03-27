using System;
using UnityEngine;

namespace Kugushev.Scripts.App.Models
{
    [Serializable]
    internal class AppModel
    {
        [SerializeField] private MainMenu mainMenu = new MainMenu();

        public MainMenu MainMenu => mainMenu; 
    }
}