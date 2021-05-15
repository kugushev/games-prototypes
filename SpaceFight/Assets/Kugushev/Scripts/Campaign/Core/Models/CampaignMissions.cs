using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Core.ValueObjects;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models
{
    public interface ICampaignMissions
    {
        IReadOnlyReactiveCollection<MissionInfo> Missions { get; }
        IReadOnlyReactiveProperty<int> Budget { get; }
    }

    internal class CampaignMissions : ICampaignMissions
    {
        private ReactiveCollection<MissionInfo>? _missions;
        private readonly ReactiveProperty<int> _budget = new ReactiveProperty<int>();

        internal void Init(IEnumerable<MissionInfo> missions, int startBudget)
        {
            if (_missions != null || _budget.Value != default)
                throw new SpaceFightException("Missions are already initialized");

            _missions = new ReactiveCollection<MissionInfo>(missions);
            _budget.Value = startBudget;
        }

        public IReadOnlyReactiveCollection<MissionInfo> Missions =>
            _missions ?? throw new SpaceFightException("Missions are not initialized");

        public IReadOnlyReactiveProperty<int> Budget => _budget;

        // todo: call via Message
        internal void OnMissionSelected()
        {
            _budget.Value -= CampaignConstants.MissionCost;
            if (_budget.Value < 0)
                Debug.LogError("Budget is less than 0");
        }

        internal void OnMissionSuccessfullyFinished(MissionInfo missionInfo) => _missions?.Remove(missionInfo);
    }
}