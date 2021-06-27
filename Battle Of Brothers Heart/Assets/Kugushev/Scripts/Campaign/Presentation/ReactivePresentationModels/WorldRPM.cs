using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Common.Core.Exceptions;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using static Kugushev.Scripts.Game.Core.GameConstants.World;
using static Kugushev.Scripts.Game.Core.Utils.WorldHelper;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels
{
    public class WorldRPM : MonoBehaviour
    {
        [Header("Tilemaps")] [SerializeField] private Tilemap ground = default!;

        [Header("Tiles")] [SerializeField] private TileBase grassTile = default!;

        [Inject] private WorldManager _worldManager = default!;
        [Inject] private CityRPM.Factory _cityFactory = default!;

        private void Awake()
        {
            if (_worldManager.Initialized)
                OnWorldManagerInitialized();
            else
                _worldManager.WorldInitialized += OnWorldManagerInitialized;
        }

        private void OnWorldManagerInitialized()
        {
            FillTiles(_worldManager.Ground);
            FillCities(_worldManager.Cities);
        }

        private void FillTiles(GroundTile[,] worldGround)
        {
            var positions = new Vector3Int[Width * Height];
            var tiles = new TileBase[Width * Height];
            int index = 0;

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                var tile = worldGround[x, y];

                positions[index] = new Vector3Int(
                    NormalizeX(x),
                    NormalizeY(y),
                    0);

                tiles[index] = tile.Type switch
                {
                    TileType.Grass => grassTile,
                    _ => throw new GameException($"Unexpected tile type {tile.Type}")
                };

                index++;
            }

            ground.SetTiles(positions, tiles);
        }

        private void FillCities(IReadOnlyList<City> cities)
        {
            foreach (var city in cities)
            {
                _cityFactory.Create(city);
                // todo: create Zenject factory
                //Instantiate(cityPrefab, position, Quaternion.identity, citiesParent);
            }
        }
    }
}