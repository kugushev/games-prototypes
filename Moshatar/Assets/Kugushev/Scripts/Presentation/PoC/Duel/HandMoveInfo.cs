using System;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public readonly struct HandMoveInfo
    {
        public Vector3 Position { get; }
        public DateTime Time { get; }

        public HandMoveInfo(Vector3 position, DateTime time)
        {
            Position = position;
            Time = time;
        }
    }
}