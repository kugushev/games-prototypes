using UnityEngine;
using UnityEngine.Tilemaps;
using static Kugushev.Scripts.CampaignConstants.World;

namespace Kugushev.Scripts.Views
{
    public class WorldView : MonoBehaviour
    {
        [Header("Scene")] [SerializeField] private Transform unitsRoot;
        [SerializeField] private Grid grid;

        [Header("Tilemaps")] [SerializeField] private Tilemap ground = default!;

        [Header("Tiles")] [SerializeField] private TileBase grassTile = default!;

        public Transform UnitsRoot => unitsRoot;

        public Vector3 CellToWorld(Vector2Int coords)
        {
            return grid.GetCellCenterWorld(new Vector3Int(coords.x, coords.y, 0));
        }

        private void Awake()
        {
            FillTiles();
        }

        private void FillTiles()
        {
            var positions = new Vector3Int[Width * Height];
            var tiles = new TileBase[Width * Height];
            int index = 0;

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                positions[index] = new Vector3Int(
                    NormalizeX(x),
                    NormalizeY(y),
                    0);

                tiles[index] = grassTile;

                index++;
            }

            ground.SetTiles(positions, tiles);
        }

        public static int NormalizeX(int x) => x - Width / 2;
        public static int NormalizeY(int y) => y - Height / 2;
    }
}