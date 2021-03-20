using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    public class MissionSelection
    {
        private List<MissionInfo> _missions;
        public IReadOnlyList<MissionInfo> Missions => _missions;
        public int Budget { get; private set; } = CampaignConstants.MaxBudget;
        public MissionInfo? SelectedMission { get; set; }

        public void SetMissions(List<MissionInfo> missions)
        {
            if (_missions != null)
                Debug.LogError("Missions are already specified");
            _missions = missions;
        }

        public void OnMissionSelected()
        {
            Budget -= CampaignConstants.MissionCost;
            if (Budget < 0)
                Debug.LogError("Budget is less than 0");
        }

        public void OnMissionFinished(MissionInfo missionInfo) => _missions.Remove(missionInfo);
    }
}