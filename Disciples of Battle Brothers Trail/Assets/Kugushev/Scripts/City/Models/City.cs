using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kugushev.Scripts.City.Interfaces;
using Kugushev.Scripts.City.Models.Cells;
using Kugushev.Scripts.City.Models.Interactables;
using UnityEngine;
using static Kugushev.Scripts.City.CityConstants;

namespace Kugushev.Scripts.City.Models
{
    public class City
    {
        public City()
        {
            var facilities = new List<IFacility>();
            Facilities = facilities;
            Grid = CreateGrid(facilities);
        }

        public IReadOnlyList<IReadOnlyList<CityCell>> Grid { get; }

        public IReadOnlyList<IFacility> Facilities { get; }

        private static IReadOnlyList<IReadOnlyList<CityCell>> CreateGrid(List<IFacility> facilities)
        {
            var grid = CreateEmptyGrid();

            PushExitZone(grid);

            PushHiringDek(grid, facilities);

            return grid;
        }

        private static void PushHiringDek(CityCell[][] grid, List<IFacility> facilities)
        {
            var deck = new HiringDesk(new Vector2Int(Width / 2, Height / 2));
            facilities.Add(deck);

            var cell = new InteractableCell(deck);
            grid[Width / 2][Height / 2] = cell;
            grid[Width / 2][Height / 2 + 1] = cell;
        }

        private static void PushExitZone(CityCell[][] grid)
        {
            var exitCell = new InteractableCell(new ExitZone());

            for (int i = 0; i < Width; i++)
            {
                grid[0][i] = exitCell;
                grid[Height - 1][i] = exitCell;
            }

            for (int i = 0; i < Height; i++)
            {
                grid[i][0] = exitCell;
                grid[i][Width - 1] = exitCell;
            }
        }

        private static CityCell[][] CreateEmptyGrid()
        {
            var grid = new CityCell[Height][];

            for (int y = 0; y < Height; y++)
            {
                grid[y] = new CityCell[Width];
                for (int x = 0; x < Width; x++)
                    grid[y][x] = EmptyPavementCell.Instance;
            }

            return grid;
        }
    }
}