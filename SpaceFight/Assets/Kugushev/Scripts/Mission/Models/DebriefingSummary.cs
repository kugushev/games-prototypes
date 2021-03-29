using System.Collections.Generic;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Perks.Abstractions;

namespace Kugushev.Scripts.Mission.Models
{
    public class DebriefingSummary : Poolable<DebriefingSummary.State>
    {
        public struct State
        {
            public PerkInfo? SelectedAchievement;
        }

        private readonly List<BasePerk> _allAchievements = new List<BasePerk>(64);

        public DebriefingSummary(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public IReadOnlyList<BasePerk> AllAchievements => _allAchievements;

        public PerkInfo? SelectedAchievement
        {
            get => ObjectState.SelectedAchievement;
            set => ObjectState.SelectedAchievement = value;
        }

        public void Fill(IReadOnlyCollection<BasePerk> allAchievements)
        {
            foreach (var achievement in allAchievements)
                _allAchievements.Add(achievement);
        }

        protected override void OnClear(State state) => _allAchievements.Clear();
        protected override void OnRestore(State state) => _allAchievements.Clear();
    }
}