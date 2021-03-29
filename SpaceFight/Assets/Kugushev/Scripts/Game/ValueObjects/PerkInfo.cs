using Kugushev.Scripts.Game.Enums;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct PerkInfo
    {
        public PerkInfo(PerkId id, int? level, PerkType type, string caption, string requirement,
            string effect)
        {
            Id = id;
            Level = level;
            Type = type;
            Caption = caption;
            Requirement = requirement;
            Effect = effect;
            Description = $"{requirement}. Effect: {effect}";
        }

        public PerkId Id { get; }
        public int? Level { get; }
        public PerkType Type { get; }
        public string Caption { get; }
        public string Requirement { get; }
        public string Effect { get; }
        public string Description { get; }

        public override string ToString() => $"{Id}. Lvl {Level}";

        #region Equality

        public bool Equals(PerkInfo other) => Id == other.Id && Level == other.Level;

        public override bool Equals(object obj) => obj is PerkInfo other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Id * 397) ^ Level.GetHashCode();
            }
        }

        public static bool operator ==(PerkInfo left, PerkInfo right) => left.Equals(right);

        public static bool operator !=(PerkInfo left, PerkInfo right) => !left.Equals(right);

        #endregion
    }
}