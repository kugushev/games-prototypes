using UnityEngine;

namespace Kugushev.Scripts.Game.Interfaces
{
    public interface IGrid
    {
        Vector3 CellToWorld(Vector2Int coords);
    }
}