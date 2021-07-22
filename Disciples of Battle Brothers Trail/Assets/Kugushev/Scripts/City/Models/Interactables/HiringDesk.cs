using Kugushev.Scripts.City.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.City.Models.Interactables
{
    public class HiringDesk: IInteractable, IFacility
    {
        public HiringDesk(Vector2Int pivotCell)
        {
            PivotCell = pivotCell;
        }

        public Vector2Int PivotCell { get; }
    }
}