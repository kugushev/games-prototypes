using System;
using UnityEngine;

namespace Kugushev.Scripts.Common.ValueObjects
{
    [Serializable]
    public readonly struct Position
    {
        public Vector3 Point { get; }

        public Position(Vector3 point) => Point = point;
    }
}