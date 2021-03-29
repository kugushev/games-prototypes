using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Interfaces;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    public class MissionSelection : Poolable<MissionSelection.State>, IMissionsSet
    {
        public struct State
        {
            public int Budget;
            public MissionInfo? SelectedMission;

            public State(int budget)
            {
                Budget = budget;
                SelectedMission = null;
            }
        }

        public MissionSelection(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        private readonly List<MissionInfo> _missions = new List<MissionInfo>(CampaignConstants.MissionsCount);
        public IReadOnlyList<MissionInfo> Missions => _missions;
        public int Budget => ObjectState.Budget;

        public MissionInfo? SelectedMission
        {
            get => ObjectState.SelectedMission;
            set => ObjectState.SelectedMission = value;
        }

        public void AddMission(MissionInfo mission) => _missions.Add(mission);

        public void OnMissionSelected()
        {
            ObjectState.Budget -= CampaignConstants.MissionCost;
            if (ObjectState.Budget < 0)
                Debug.LogError("Budget is less than 0");
        }

        public void OnMissionFinished(MissionInfo missionInfo) => _missions.Remove(missionInfo);


        protected override void OnClear(State state) => _missions.Clear();
        protected override void OnRestore(State state) => _missions.Clear();
    }
}