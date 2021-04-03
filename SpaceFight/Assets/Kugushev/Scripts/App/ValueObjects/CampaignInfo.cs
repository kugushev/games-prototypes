using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Enums;
using UnityEngine;

namespace Kugushev.Scripts.App.ValueObjects
{
    public readonly struct CampaignInfo
    {
        public CampaignInfo(int seed, int? budget, IReadOnlyList<PerkId> availablePerks, bool isPlayground,
            bool isStandalone)
        {
            IsPlayground = isPlayground;
            IsStandalone = isStandalone;
            AvailablePerks = availablePerks;
            Seed = seed;
            Budget = budget;
        }

        public int Seed { get; }
        public bool IsPlayground { get; }
        public IReadOnlyList<PerkId> AvailablePerks { get; }
        public int? Budget { get; }
        public bool IsStandalone { get; }
    }
}