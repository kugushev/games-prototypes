namespace Kugushev.Scripts.Game.Core.Utils
{
    public static class WorldHelper
    {
        public static int NormalizeX(int x) => x - GameConstants.World.Width / 2;
        public static int NormalizeY(int y) => y - GameConstants.World.Height / 2;
    }
}