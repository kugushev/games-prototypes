using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Achievements;
using Kugushev.Scripts.Mission.Achievements.Abstractions;

namespace Kugushev.Scripts.Mission.Models
{
    public class DebriefingInfo : Poolable<DebriefingInfo.State>
    {
        public struct State
        {
            public AchievementId? SelectedAchievement;
        }

        private readonly List<AbstractAchievement> _allAchievements = new List<AbstractAchievement>(64);

        public DebriefingInfo(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public IReadOnlyList<AbstractAchievement> AllAchievements => _allAchievements;

        public AchievementId? SelectedAchievement
        {
            get => ObjectState.SelectedAchievement;
            set => ObjectState.SelectedAchievement = value;
        }

        public void Fill(IReadOnlyCollection<AbstractAchievement> allAchievements)
        {
            foreach (var achievement in allAchievements)
                _allAchievements.Add(achievement);
        }

        protected override void OnClear(State state) => _allAchievements.Clear();
        protected override void OnRestore(State state) => _allAchievements.Clear();
    }
}