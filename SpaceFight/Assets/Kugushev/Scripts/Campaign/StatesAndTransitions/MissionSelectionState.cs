using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Services;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    public class MissionSelectionState : BaseSceneLoadingState<MissionSelection>
    {
        private readonly MissionsGenerationService _missionsGenerationService;

        public MissionSelectionState(MissionSelection model, MissionsGenerationService missionsGenerationService)
            : base(model, UnityConstants.MissionSelectionScene, true)
        {
            _missionsGenerationService = missionsGenerationService;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.Missions = _missionsGenerationService.GenerateMissions();
        }
    }
}