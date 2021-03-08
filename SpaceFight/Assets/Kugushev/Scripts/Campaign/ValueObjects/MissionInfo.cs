using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    [Serializable]
    public struct MissionInfo
    {
        [SerializeField] private int seed;

        public MissionInfo(int seed, IReadOnlyList<AchievementInfo> playerAchievements)
        {
            PlayerAchievements = playerAchievements;
            this.seed = seed;
        }

        public int Seed => seed;
        public IReadOnlyList<AchievementInfo> PlayerAchievements { get; }
    }
}