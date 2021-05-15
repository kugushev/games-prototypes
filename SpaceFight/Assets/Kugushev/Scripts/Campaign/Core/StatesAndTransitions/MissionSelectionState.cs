using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class MissionSelectionState : BaseSceneLoadingState<CampaignModelOld>
    {
        // private readonly MissionsGenerator _missionsGenerator;

        public MissionSelectionState(CampaignModelOld modelOld, object missionsGenerationService)
            : base(modelOld, UnityConstants.MissionSelectionScene, true)
        {
            // _missionsGenerator = missionsGenerationService;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var modelMissionSelection = ModelOld.MissionSelection;

            ModelOld.NextMission = null;
            // if (modelMissionSelection.Missions.Count == 0)
            //     _missionsGenerator.GenerateMissions(modelMissionSelection);

            if (ModelOld.LastMissionResult is {PlayerWins: true} lastMissionResult)
                modelMissionSelection.OnMissionFinished(lastMissionResult.MissionInfo);
        }

        protected override void OnExitBeforeUnloadScene()
        {
            ModelOld.NextMission = ModelOld.MissionSelection.SelectedMission;
            if (ModelOld.NextMission != null)
                ModelOld.MissionSelection.OnMissionSelected();
        }
    }
}