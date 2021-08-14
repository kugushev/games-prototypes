using UnityEngine;

namespace Kugushev.Scripts.Game.Components
{
    public readonly struct UnitMoveOneStepEvent
    {
        public UnitMoveOneStepEvent(Vector2Int actualPosition, Vector2Int previousPosition)
        {
            PreviousPosition = previousPosition;
            ActualPosition = actualPosition;
        }

        public readonly Vector2Int ActualPosition;

        public readonly Vector2Int PreviousPosition;
    }
}