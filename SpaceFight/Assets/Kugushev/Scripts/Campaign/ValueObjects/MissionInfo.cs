using System;
using Kugushev.Scripts.Campaign.Models;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    [Serializable]
    public struct MissionInfo
    {
        [SerializeField] private int seed;

        public MissionInfo(int seed, PlayerAchievements playerAchievements)
        {
            PlayerAchievements = playerAchievements;
            this.seed = seed;
        }

        public int Seed => seed;
        public PlayerAchievements PlayerAchievements { get; }
    }
}