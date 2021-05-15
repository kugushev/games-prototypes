using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core.Models
{
    public interface ICampaignMissions
    {
        IReadOnlyReactiveCollection<MissionInfo> Missions { get; }
        IReadOnlyReactiveProperty<int> Budget { get; }
    }

    internal class CampaignMissions : ICampaignMissions
    {
        private readonly SignalBus _signalBus;
        private readonly ObtainIntrigueCard.Factory _obtainIntrigueSignalFactory;
        private readonly ReactiveProperty<int> _budget = new ReactiveProperty<int>();
        private ReactiveCollection<MissionInfo>? _missions;

        public CampaignMissions(SignalBus signalBus, ObtainIntrigueCard.Factory obtainIntrigueSignalFactory)
        {
            _signalBus = signalBus;
            _obtainIntrigueSignalFactory = obtainIntrigueSignalFactory;
        }

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

        internal void OnMissionSelected()
        {
            _budget.Value -= CampaignConstants.MissionCost;
            if (_budget.Value < 0)
                Debug.LogError("Budget is less than 0");
        }

        internal void OnMissionFinished(MissionInfo missionInfo, bool playerWins)
        {
            if (playerWins)
            {
                _missions?.Remove(missionInfo);

                var signal = _obtainIntrigueSignalFactory.Create(missionInfo.Reward);
                _signalBus.Fire(signal);
            }
        }
    }
}