using System;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    [Serializable]
    public struct CampaignInfo
    {
        [SerializeField] private int seed;

        public CampaignInfo(int seed)
        {
            this.seed = seed;
        }

        public int Seed => seed;
        public bool IsPlaygroundMode => true;
    }
}