using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ProceduralGeneration;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class MissionSelectionState : BaseSceneLoadingState<CampaignModel>
    {
        private readonly MissionsGenerator _missionsGenerator;

        public MissionSelectionState(CampaignModel model, MissionsGenerator missionsGenerationService)
            : base(model, UnityConstants.MissionSelectionScene, true)
        {
            _missionsGenerator = missionsGenerationService;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var modelMissionSelection = Model.MissionSelection;

            Model.NextMission = null;
            if (modelMissionSelection.Missions.Count == 0)
                _missionsGenerator.GenerateMissions(modelMissionSelection);

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