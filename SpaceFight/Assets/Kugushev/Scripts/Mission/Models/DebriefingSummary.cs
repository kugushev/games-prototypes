using System.Collections.Generic;
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

        private readonly List<BasePerk> _allPerks = new List<BasePerk>(64);

        public DebriefingSummary(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public IReadOnlyList<BasePerk> AllPerks => _allPerks;

        public PerkInfo? SelectedAchievement
        {
            get => ObjectState.SelectedAchievement;
            set => ObjectState.SelectedAchievement = value;
        }

        public void Fill(IReadOnlyCollection<BasePerk> allPerks)
        {
            foreach (var achievement in allPerks)
                _allPerks.Add(achievement);
        }

        protected override void OnClear(State state) => _allPerks.Clear();
        protected override void OnRestore(State state) => _allPerks.Clear();
    }
}