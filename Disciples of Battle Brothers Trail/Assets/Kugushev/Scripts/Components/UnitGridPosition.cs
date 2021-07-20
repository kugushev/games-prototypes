using Kugushev.Scripts.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Components
{
    public struct UnitGridPosition
    {
        public Vector2Int PreviousPosition;
        public Vector2Int ActualPosition;
        public Direction2d Direction;
        public bool Moving;
        public bool Stopped;
    }
}