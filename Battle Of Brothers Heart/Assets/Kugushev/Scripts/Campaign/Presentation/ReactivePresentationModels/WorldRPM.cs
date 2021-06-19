using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Core.Enums;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Tiles;
using Kugushev.Scripts.Common.Core.Exceptions;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using static Kugushev.Scripts.Campaign.Core.CampaignConstants.World;
using static Kugushev.Scripts.Campaign.Presentation.Helpers.WorldHelper;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels
{
    public class WorldRPM : MonoBehaviour
    {
        [Header("Tilemaps")] [SerializeField] private Tilemap ground = default!;

        [Header("Tiles")] [SerializeField] private TileBase grassTile = default!;

        [Header("Cities")] [SerializeField] private Transform citiesParent = default!;
        [SerializeField] private GameObject cityPrefab = default!;

        [Inject] private World _world = default!;
        [Inject] private CityRPM.Factory _cityFactory = default!;

        private void Awake()
        {
            if (_world.Initialized)
                OnWorldInitialized();
            else
                _world.WorldInitialized += OnWorldInitialized;
        }

        private void OnWorldInitialized()
        {
            FillTiles(_world.Ground);
            FillCities(_world.Cities);
        }

        private void FillTiles(GroundTile[,] worldGround)
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                var tile = worldGround[x, y];

                var tileBase = tile.Type switch
                {
                    TileType.Grass => grassTile,
                    _ => throw new GameException($"Unexpected tile type {tile.Type}")
                };

                var position = new Vector3Int(
                    NormalizeX(x),
                    NormalizeY(y),
                    0);

                ground.SetTile(position, tileBase);
            }
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