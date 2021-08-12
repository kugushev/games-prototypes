namespace Kugushev.Scripts.Game
{
    internal static class GameConstants
    {
        public static class Units
        {
            public const float Speed = 20f;
        }

        public static class World
        {
            public const int Height = 64;
            public const int Width = 64;

            public const int TilesPerCellHeight = 8;
            public const int TilesPerCellWidth = 8;

            public const int CitiesInHorizontal = 4;
            public const int CitiesInVertical = 3;
            public const int CityAreaRatioDivider = 4;
        }
    }
}