using System;
using Kugushev.Scripts.Game.Constants;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [Serializable]
    internal class MainMenu
    {
        [SerializeField] private int seed = GameConstants.DefaultCampaignSeed;

        public int Seed
        {
            get => seed;
            set => seed = value;
        }
    }
}