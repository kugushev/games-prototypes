using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Utils;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    public class CampaignState : BaseSceneLoadingState<GameModel>
    {
        private readonly CampaignSceneParametersPipeline _campaignSceneParametersPipeline;
        private readonly CampaignSceneResultPipeline _campaignSceneResultPipeline;

        public CampaignState(GameModel model, CampaignSceneParametersPipeline campaignSceneParametersPipeline,
            CampaignSceneResultPipeline campaignSceneResultPipeline)
            : base(model, UnityConstants.CampaignManagementScene, false)
        {
            _campaignSceneParametersPipeline = campaignSceneParametersPipeline;
            _campaignSceneResultPipeline = campaignSceneResultPipeline;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var (budget, perks) = Model.CampaignPreparation.PrepareCampaign();

            var campaignSeed = Random.Range(0, 100); // just for test purposes

            _campaignSceneParametersPipeline.Set(new CampaignInfo(
                campaignSeed,
                budget,
                perks,
                false,
                false
            ));
        }

        protected override void OnExitBeforeUnloadScene()
        {
            var result = _campaignSceneResultPipeline.Get();
            Model.AddPoliticalActions(result.CampaignResult.RewardedPoliticalActions);
            result.CampaignResult.Dispose();
        }
    }
}