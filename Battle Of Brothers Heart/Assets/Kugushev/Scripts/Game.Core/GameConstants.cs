using System;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core
{
    public static class GameConstants
    {
        public static class Units
        {
            public static readonly Position PlayerUnitStartPosition = new Position(new Vector2(0, 0));
            public static readonly Direction2d PlayerUnitStartDirection = Direction2d.Down;
            public static readonly TimeSpan WinFreezeDuration = TimeSpan.FromSeconds(10);
        }

        public static class Characters
        {
            public const int DefaultMaxHitPoints = 20;
        }

        public static class World
        {
            public const int Height = 480;
            public const int Width = 640;

            public const int CitiesInHorizontal = 4;
            public const int CitiesInVertical = 3;
            public const int CityAreaRatioDivider = 4;

            public const int BanditsPerCityMin = 2;
            public const int BanditsPerCityMax = 7;
            public const int BanditsPowerMin = 1;
            public const int BanditsPowerMax = 4;
        }
    }
}