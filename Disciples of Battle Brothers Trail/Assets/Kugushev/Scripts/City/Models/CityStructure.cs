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
    public class CityStructure
    {
        private readonly Game.Models.CityInfo.CityWorldItem _cityWorldItemInfo;
        private readonly IReadOnlyList<IReadOnlyList<CityCell>> _grid;

        public CityStructure(Game.Models.CityInfo.CityWorldItem cityWorldItemInfo)
        {
            _cityWorldItemInfo = cityWorldItemInfo;
            var facilities = new List<IFacility>();
            Facilities = facilities;
            _grid = CreateGrid(facilities);
        }

        public IReadOnlyList<IFacility> Facilities { get; }

        public CityCell GetCell(Vector2Int coords) => _grid[coords.y][coords.x];

        private IReadOnlyList<IReadOnlyList<CityCell>> CreateGrid(List<IFacility> facilities)
        {
            var grid = CreateEmptyGrid();

            PushExitZone(grid);

            PushHiringDek(grid, facilities);

            return grid;
        }

        private void PushHiringDek(CityCell[][] grid, List<IFacility> facilities)
        {
            var deck = new HiringDesk(new Vector2Int(Width / 2, Height / 2), _cityWorldItemInfo.HiringDeskInfo);
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