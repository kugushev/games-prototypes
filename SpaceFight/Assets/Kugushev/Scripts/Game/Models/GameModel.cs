using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Constants;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + "GameModel")]
    public class GameModel: ScriptableObject
    {
        [SerializeField] private MainMenu mainMenu;

        public MainMenu MainMenu => mainMenu; 
        public object CurrentCampaign { get; set; }
    }
}