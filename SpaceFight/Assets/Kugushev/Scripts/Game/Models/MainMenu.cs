using System;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [Serializable]
    internal class MainMenu
    {
        [SerializeField] private bool startClicked;
        [SerializeField] private int seed = 42;

        public bool StartClicked
        {
            get => startClicked;
            set => startClicked = value;
        }

        public int Seed
        {
            get => seed;
            set => seed = value;
        }
    }
}