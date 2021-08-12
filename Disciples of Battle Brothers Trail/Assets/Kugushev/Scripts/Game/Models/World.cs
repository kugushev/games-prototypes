using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Models.CityInfo;
using UnityEngine;
using static Kugushev.Scripts.Game.GameConstants.World;
using Random = UnityEngine.Random;


namespace Kugushev.Scripts.Game.Models
{
    public class World
    {
        private readonly IReadOnlyList<IReadOnlyList<WorldCell>> _grid;

        public World()
        {
            var seed = DateTime.Now.Millisecond;
            Debug.Log($"Seed {seed}");
            Random.InitState(seed);

            _grid = CreateWorld();
        }

        public WorldCell GetCell(int x, int y) => _grid[y][x];

        public WorldCell GetCell(Vector2Int vector) => _grid[vector.y][vector.x];

        private WorldCell[][] CreateWorld()
        {
            var tiles = FillGround();
            PushCities(tiles);

            return tiles;
        }

        private static WorldCell[][] FillGround()
        {
            var ground = new WorldCell[Height][];
            for (int y = 0; y < Height; y++)
            {
                ground[y] = new WorldCell[Width];
                for (int x = 0; x < Width; x++)
                    ground[y][x] = GrasslandWorldCell.Instance;
            }

            return ground;
        }

        private void PushCities(WorldCell[][] grid)
        {
            int areaWidth = Width / CitiesInHorizontal;
            int areaHeight = Height / CitiesInVertical;

            for (int areaX = 0; areaX < CitiesInHorizontal; areaX++)
            for (int areaY = 0; areaY < CitiesInVertical; areaY++)
            {
                var areaBorderX = areaWidth / CityAreaRatioDivider;
                int fromX = areaX * areaWidth + areaBorderX;
                int toX = (areaX + 1) * areaWidth - areaBorderX;
                int x = Random.Range(fromX, toX);

                var areaBorderY = areaHeight / CityAreaRatioDivider;
                int fromY = areaY * areaHeight + areaBorderY;
                int toY = (areaY + 1) * areaHeight - areaBorderY;
                int y = Random.Range(fromY, toY);

                var city = new CityWorldItem();
                grid[y][x] = new CityWorldCell(city);
            }
        }
    }
}