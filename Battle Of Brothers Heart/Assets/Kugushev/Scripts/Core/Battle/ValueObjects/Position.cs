using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.ValueObjects
{
    public readonly struct Position
    {
        public Vector2 Vector { get; }

        public Position(Vector2 vector)
        {
            Vector = vector;
        }
    }
}