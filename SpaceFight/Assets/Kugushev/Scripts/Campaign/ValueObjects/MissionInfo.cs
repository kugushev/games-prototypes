using System;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    [Serializable]
    public struct MissionInfo
    {
        [SerializeField] private int seed;

        public MissionInfo(int seed)
        {
            this.seed = seed;
        }

        public int Seed => seed;
    }
}