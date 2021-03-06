using System;
using Kugushev.Scripts.Common.ValueObjects;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public readonly struct Sun
    {
        public Sun(Position position, float size)
        {
            Position = position;
            Size = size;
        }

        public readonly Position Position;
        public readonly float Size;

        public float Radius => 0.5f * Size;

        #region Equality

        public bool Equals(Sun other) => Position.Equals(other.Position) && Size.Equals(other.Size);

        public override bool Equals(object obj) => obj is Sun other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Size.GetHashCode();
            }
        }

        public static bool operator ==(Sun left, Sun right) => left.Equals(right);

        public static bool operator !=(Sun left, Sun right) => !left.Equals(right);

        #endregion
    }
}