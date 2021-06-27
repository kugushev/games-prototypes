using Kugushev.Scripts.Game.Core.Enums;

namespace Kugushev.Scripts.Game.Core.ValueObjects.Tiles
{
    public readonly struct GroundTile
    {
        public GroundTile(TileType type)
        {
            Type = type;
        }

        public TileType Type { get; }
    }
}