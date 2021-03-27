using System;
using Kugushev.Scripts.App.Constants;
using UnityEngine;

namespace Kugushev.Scripts.App.Models
{
    [Serializable]
    internal class MainMenu
    {
        [SerializeField] private int seed = AppConstants.DefaultCampaignSeed;

        public int Seed
        {
            get => seed;
            set => seed = value;
        }
    }
}