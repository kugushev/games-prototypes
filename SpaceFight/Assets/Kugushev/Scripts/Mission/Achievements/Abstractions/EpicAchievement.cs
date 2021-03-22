using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Abstractions
{
    public abstract class EpicAchievement : AbstractAchievement
    {
        [SerializeField] private int level;

        private AchievementInfo? _info;

        public sealed override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId, level, AchievementType.Epic, Name,
            Criteria,
            Perk);

        protected abstract AchievementId AchievementId { get; }
        protected abstract string Name { get; }
        protected abstract string Criteria { get; }
        protected abstract string Perk { get; }
    }
}