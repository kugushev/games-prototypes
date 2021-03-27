using Kugushev.Scripts.App.Enums;

namespace Kugushev.Scripts.App.ValueObjects
{
    public readonly struct AchievementInfo
    {
        public AchievementInfo(AchievementId id, int? level, AchievementType type, string caption, string criteria,
            string perk)
        {
            Id = id;
            Level = level;
            Type = type;
            Caption = caption;
            Description = $"{criteria}. Effect: {perk}";
        }

        public AchievementId Id { get; }
        public int? Level { get; }
        public AchievementType Type { get; }
        public string Caption { get; }
        public string Description { get; }

        public override string ToString() => $"{Id}. Lvl {Level}";

        #region Equality

        public bool Equals(AchievementInfo other) => Id == other.Id && Level == other.Level;

        public override bool Equals(object obj) => obj is AchievementInfo other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Id * 397) ^ Level.GetHashCode();
            }
        }

        public static bool operator ==(AchievementInfo left, AchievementInfo right) => left.Equals(right);

        public static bool operator !=(AchievementInfo left, AchievementInfo right) => !left.Equals(right);

        #endregion
    }
}