using System;
using Kugushev.Scripts.City.Interfaces;
using Kugushev.Scripts.City.Models.Cells;
using Kugushev.Scripts.City.Models.Interactables;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Kugushev.Scripts.City.CityConstants;

namespace Kugushev.Scripts.City.Views
{
    public class CityView : MonoBehaviour, IGrid
    {
        [SerializeField] private Grid grid;

        [Header("Tilemaps")] [SerializeField] private Tilemap ground;
        [SerializeField] private Tilemap surface;

        [Header("Tiles")] [SerializeField] private TileBase pavementTile;
        [SerializeField] private TileBase exitZoneTile;

        [Header("Facilities")] [SerializeField]
        private Transform facilitiesRoot;

        [SerializeField] private GameObject hiringDeskPrefab;

        public Vector3 CellToWorld(Vector2Int coords)
        {
            var x = NormalizeX(coords.x);
            var y = NormalizeY(coords.y);
            return grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
        }

        public void Init(Models.CityStructure cityStructure)
        {
            DrawTiles(cityStructure);
            ArrangeFacilities(cityStructure);
        }

        private void DrawTiles(Models.CityStructure cityStructure)
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                var cell = cityStructure.GetCell(new Vector2Int(x, y));

                var position = new Vector3Int(
                    NormalizeX(x),
                    NormalizeY(y),
                    0);

                ground.SetTile(position, pavementTile);

                switch (cell)
                {
                    case EmptyPavementCell _:
                        break;
                    case InteractableCell interactable:
                        switch (interactable.Interactable)
                        {
                            case ExitZone _:
                                surface.SetTile(position, exitZoneTile);
                                break;
                            case IFacility _:
                                // we'll put facilities later on pavement
                                break;
                            default:
                                Debug.LogError($"Unexpected interactable {interactable.Interactable}");
                                break;
                        }

                        break;
                    default:
                        throw new Exception($"Unexpected cell {cell}");
                }
            }
        }

        private void ArrangeFacilities(Models.CityStructure cityStructure)
        {
            foreach (var facility in cityStructure.Facilities)
            {
                var position = new Vector3Int(
                    NormalizeX(facility.PivotCell.x),
                    NormalizeY(facility.PivotCell.y),
                    0);

                switch (facility)
                {
                    case HiringDesk _:
                        Instantiate(hiringDeskPrefab, position, Quaternion.identity, facilitiesRoot);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(facility));
                }
            }
        }

        private static int NormalizeX(int x) => x - Width / 2;
        private static int NormalizeY(int y) => y - Height / 2;
    }
}