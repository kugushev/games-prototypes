using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Enums;
using UnityEngine;

namespace Kugushev.Scripts.App.ValueObjects
{
    [Serializable]
    public struct CampaignInfo
    {
        [SerializeField] private int seed;
        [SerializeField] private int? budget;
        [SerializeField] private bool isPlayground;

        public CampaignInfo(int seed, int? budget, IReadOnlyList<PerkId> availablePerks, bool isPlayground)
        {
            AvailablePerks = availablePerks;
            this.isPlayground = isPlayground;
            this.seed = seed;
            this.budget = budget;
        }


        public int Seed => seed;
        public bool IsPlayground => isPlayground;
        public IReadOnlyList<PerkId> AvailablePerks { get; }
        public int? Budget => budget;
    }
}