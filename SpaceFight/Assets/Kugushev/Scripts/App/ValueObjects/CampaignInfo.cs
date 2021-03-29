using System;
using UnityEngine;

namespace Kugushev.Scripts.App.ValueObjects
{
    [Serializable]
    public struct CampaignInfo
    {
        [SerializeField] private int seed;
        [SerializeField] private int? budget;
        [SerializeField] private bool isPlayground;

        public CampaignInfo(int seed, int? budget, bool isPlayground)
        {
            this.isPlayground = isPlayground;
            this.seed = seed;
            this.budget = budget;
        }

        public int Seed => seed;
        public bool IsPlayground => isPlayground;

        public int? Budget => budget;
    }
}