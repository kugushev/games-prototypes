using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;
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
        private readonly IIntrigues _intrigues;
        private readonly IPlayerPerks _playerPerks;
        private readonly ReactiveProperty<int> _budget = new ReactiveProperty<int>();
        private ReactiveCollection<MissionInfo>? _missions;

        public CampaignMissions(IIntrigues intrigues,
            IPlayerPerks playerPerks)
        {
            _intrigues = intrigues;
            _playerPerks = playerPerks;
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

        internal void OnMissionFinished(MissionExitParameters parameters)
        {
            if (parameters.PlayerWins)
            {
                _missions?.Remove(parameters.MissionInfo);

                if (parameters.ChosenPerk != null)
                    _playerPerks.ObtainPerk(parameters.ChosenPerk.Value);

                _intrigues.ObtainCard(parameters.MissionInfo.Reward);
            }
        }
    }
}