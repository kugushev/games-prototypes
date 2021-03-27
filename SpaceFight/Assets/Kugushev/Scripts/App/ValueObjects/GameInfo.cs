using UnityEngine;

namespace Kugushev.Scripts.App.ValueObjects
{
    public readonly struct GameInfo
    {
        public readonly int Seed;

        public GameInfo(int seed)
        {
            Seed = seed;
        }
    }
}