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
    }
}