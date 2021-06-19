using System;
using Kugushev.Scripts.Campaign.Core;
using Kugushev.Scripts.Campaign.Core.Enums;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Tiles;
using Kugushev.Scripts.Common.Core.Exceptions;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using static Kugushev.Scripts.Campaign.Core.CampaignConstants.World;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels
{
    public class WorldRPM : MonoBehaviour
    {
        [Header("Tilemaps")] [SerializeField] private Tilemap ground = default!;

        [Header("Tiles")] [SerializeField] private TileBase grassTile = default!;

        [Inject] private World _world = default!;

        private void Awake()
        {
            if (_world.Ground != null)
                FillTiles(_world.Ground);
            else
                _world.WorldInitialized += FillTiles;
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
                    x - Width / 2,
                    y - Height / 2,
                    0);

                ground.SetTile(position, tileBase);
            }
        }
    }
}