using System;
using Kugushev.Scripts.Game.Constants;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    [Serializable]
    internal class MainMenu
    {
        [SerializeField] private bool readyToStartCampaign;
        [SerializeField] private int seed = GameConstants.DefaultCampaignSeed;

        public bool ReadyToStartCampaign
        {
            get => readyToStartCampaign;
            set => readyToStartCampaign = value;
        }

        public int Seed
        {
            get => seed;
            set => seed = value;
        }
    }
}