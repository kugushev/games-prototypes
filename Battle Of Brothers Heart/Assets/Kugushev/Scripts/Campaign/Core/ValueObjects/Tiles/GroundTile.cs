using Kugushev.Scripts.Campaign.Core.Enums;

namespace Kugushev.Scripts.Campaign.Core.ValueObjects.Tiles
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