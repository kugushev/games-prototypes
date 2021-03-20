using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Services;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class MissionSelectionState : BaseSceneLoadingState<CampaignModel>
    {
        private readonly MissionsGenerationService _missionsGenerationService;

        public MissionSelectionState(CampaignModel model, MissionsGenerationService missionsGenerationService)
            : base(model, UnityConstants.MissionSelectionScene, true)
        {
            _missionsGenerationService = missionsGenerationService;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var modelMissionSelection = Model.MissionSelection;

            Model.NextMission = null;
            if (modelMissionSelection.Missions == null)
            {
                var missions = _missionsGenerationService.GenerateMissions();
                modelMissionSelection.SetMissions(missions);
            }

            if (Model.LastMissionResult is {PlayerWins: true} lastMissionResult)
                modelMissionSelection.OnMissionFinished(lastMissionResult.MissionInfo);
        }

        protected override void OnExitBeforeUnloadScene()
        {
            Model.NextMission = Model.MissionSelection.SelectedMission;
            if (Model.NextMission != null)
                Model.MissionSelection.OnMissionSelected();
        }
    }
}