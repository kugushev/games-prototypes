using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Constants;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [Serializable]
    internal class GameModel
    {
        [SerializeField] private MainMenu mainMenu = new MainMenu();

        public MainMenu MainMenu => mainMenu; 
    }
}