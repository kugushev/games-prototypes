using System;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct PoliticalActionInfo
    {
        public PoliticalAction PoliticalAction { get; }
        private readonly Guid _id;

        public PoliticalActionInfo(PoliticalAction politicalAction)
        {
            PoliticalAction = politicalAction;
            _id = Guid.NewGuid();
        }

        public bool Equals(PoliticalActionInfo other) => _id.Equals(other._id);

        public override bool Equals(object obj) => obj is PoliticalActionInfo other && Equals(other);

        public override int GetHashCode() => _id.GetHashCode();

        public static bool operator ==(PoliticalActionInfo left, PoliticalActionInfo right) => left.Equals(right);

        public static bool operator !=(PoliticalActionInfo left, PoliticalActionInfo right) => !left.Equals(right);
    }
}