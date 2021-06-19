using System;
using Kugushev.Scripts.Campaign.Core.Enums;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Tiles;
using Zenject;
using static Kugushev.Scripts.Campaign.Core.CampaignConstants.World;

namespace Kugushev.Scripts.Campaign.Core.Models
{
    public class World : IInitializable
    {
        private GroundTile[,]? _ground;

        public event Action<GroundTile[,]>? WorldInitialized;

        public GroundTile[,]? Ground => _ground;

        void IInitializable.Initialize()
        {
            _ground = CreateWorld();
            WorldInitialized?.Invoke(_ground);
        }

        private GroundTile[,] CreateWorld()
        {
            var ground = new GroundTile[Width, Height];
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                ground[x, y] = new GroundTile(TileType.Grass);

            return ground;
        }
    }
}