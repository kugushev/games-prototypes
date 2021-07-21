using System;
using System.Collections.Generic;
using Kugushev.Scripts.Models;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Kugushev.Scripts.CampaignConstants.World;

namespace Kugushev.Scripts.Views
{
    public class WorldView : MonoBehaviour
    {
        [Header("Scene")] [SerializeField] private Transform unitsRoot;
        [SerializeField] private Grid grid;

        [Header("Tilemaps")] [SerializeField] private Tilemap ground;
        [SerializeField] private Tilemap surface;

        [Header("Tiles")] [SerializeField] private TileBase grassTile;
        [SerializeField] private TileBase cityTile;


        private World _world;

        public Transform UnitsRoot => unitsRoot;

        public void Init(World world)
        {
            _world = world;
            FillTiles();
        }

        public Vector3 CellToWorld(Vector2Int coords)
        {
            var x = NormalizeX(coords.x * TilesPerCellWidth + TilesPerCellWidth / 2);
            var y = NormalizeY(coords.y * TilesPerCellHeight + TilesPerCellHeight / 2);
            return grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
        }

        private void FillTiles()
        {
            var capacity = Width * TilesPerCellWidth * Height * TilesPerCellHeight;
            var groundPositions = new Vector3Int[capacity];
            var groundTiles = new TileBase[capacity];
            int index = 0;

            for (int x = 0; x < Width * TilesPerCellWidth; x++)
            for (int y = 0; y < Height * TilesPerCellHeight; y++)
            {
                int cellX = x / TilesPerCellWidth;
                int cellY = y / TilesPerCellHeight;
                var cell = _world.GetCell(cellX, cellY);

                var position = new Vector3Int(
                    NormalizeX(x),
                    NormalizeY(y),
                    0);

                switch (cell)
                {
                    case GrasslandWorldCell _:
                        groundPositions[index] = position;
                        groundTiles[index] = grassTile;
                        break;
                    case CityWorldCell _:
                        groundPositions[index] = position;
                        groundTiles[index] = grassTile;
                        
                        surface.SetTile(position, cityTile);
                        break;
                    default:
                        throw new Exception($"Unexpected cell {cell}");
                }

                index++;
            }

            ground.SetTiles(groundPositions, groundTiles);
        }
        
        public static int NormalizeX(int x) => x - Width * TilesPerCellWidth / 2;
        public static int NormalizeY(int y) => y - Height * TilesPerCellHeight / 2;
    }
}