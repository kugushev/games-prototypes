using System;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct IntrigueRecord : IEquatable<IntrigueRecord>
    {
        public Intrigue Intrigue { get; }
        private readonly Guid _id;

        public IntrigueRecord(Intrigue intrigue)
        {
            Intrigue = intrigue;
            _id = Guid.NewGuid();
        }

        public bool Equals(IntrigueRecord other) => _id.Equals(other._id);

        public override bool Equals(object obj) => obj is IntrigueRecord other && Equals(other);

        public override int GetHashCode() => _id.GetHashCode();

        public static bool operator ==(IntrigueRecord left, IntrigueRecord right) => left.Equals(right);

        public static bool operator !=(IntrigueRecord left, IntrigueRecord right) => !left.Equals(right);
    }
}