﻿namespace Kugushev.Scripts.App.Core.ValueObjects
{
    public readonly struct GameParameters
    {
        public readonly int Seed;

        public GameParameters(int seed)
        {
            Seed = seed;
        }
    }
}