using System;
using Kugushev.Scripts.Game.Constants;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + "MainMenu")]
    public class MainMenu : ScriptableObject
    {
        [NonSerialized] private bool _startClicked;

        public bool StartClicked
        {
            get => _startClicked;
            set => _startClicked = value;
        }
    }
}