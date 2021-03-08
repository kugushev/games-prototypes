using Kugushev.Scripts.Game.Enums;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct AchievementInfo
    {
        public AchievementInfo(AchievementId id, int level, string caption, string description)
        {
            Id = id;
            Level = level;
            Caption = caption;
            Description = description;
        }

        public AchievementId Id { get; }
        public int Level { get; }
        public string Caption { get; }
        public string Description { get; }

        #region Equality

        public bool Equals(AchievementInfo other) => Id == other.Id && Level == other.Level;

        public override bool Equals(object obj) => obj is AchievementInfo other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Id * 397) ^ Level;
            }
        }

        public static bool operator ==(AchievementInfo left, AchievementInfo right) => left.Equals(right);

        public static bool operator !=(AchievementInfo left, AchievementInfo right) => !left.Equals(right);

        #endregion
    }
}