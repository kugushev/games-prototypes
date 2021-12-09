using System;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.ValueObjects
{
    public readonly struct Position : IEquatable<Position>
    {
        public Vector2 Vector { get; }

        public Position(Vector2 vector)
        {
            Vector = vector;
        }

        public Vector3 To3D() => new Vector3
        {
            x = Vector.x,
            z = Vector.y // as intended - just translate 2D to 3D
        };

        #region Equality

        public bool Equals(Position other)
        {
            return Vector.Equals(other.Vector);
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Vector.GetHashCode();
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}