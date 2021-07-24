using Kugushev.Scripts.City.Interfaces;
using Kugushev.Scripts.Game.Models.CityInfo;
using UnityEngine;

namespace Kugushev.Scripts.City.Models.Interactables
{
    public class HiringDesk : IInteractable, IFacility
    {
        public HiringDesk(Vector2Int pivotCell, HiringDeskInfo hiringDeskInfo)
        {
            PivotCell = pivotCell;
            HiringDeskInfo = hiringDeskInfo;
        }

        public Vector2Int PivotCell { get; }
        public HiringDeskInfo HiringDeskInfo { get; }
    }
}