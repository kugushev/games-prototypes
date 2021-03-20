using System;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    [Serializable]
    public struct CampaignInfo
    {
        [SerializeField] private int seed;
        [SerializeField] private bool isPlayground;

        public CampaignInfo(int seed, bool isPlayground)
        {
            this.isPlayground = isPlayground;
            this.seed = seed;
        }

        public int Seed => seed;
        public bool IsPlayground => isPlayground;
    }
}