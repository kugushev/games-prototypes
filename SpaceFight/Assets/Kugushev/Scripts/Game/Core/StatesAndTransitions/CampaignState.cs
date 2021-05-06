using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Utils;

namespace Kugushev.Scripts.Game.Core.StatesAndTransitions
{
    internal class CampaignState : BaseSceneLoadingState<GameModel>
    {
        private readonly CampaignSceneResultPipeline _campaignSceneResultPipeline;

        public CampaignState(GameModel model, CampaignSceneResultPipeline campaignSceneResultPipeline)
            : base(model, UnityConstants.CampaignManagementScene, false)
        {
            _campaignSceneResultPipeline = campaignSceneResultPipeline;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            // var (budget, perks) = Model.CampaignPreparation.PrepareCampaign();

            // var campaignSeed = Random.Range(0, 100); // just for test purposes

            // _campaignSceneParametersPipeline.Set(new CampaignInfo(
            //     campaignSeed,
            //     budget,
            //     perks,
            //     false,
            //     false
            // ));
        }

        protected override void OnExitBeforeUnloadScene()
        {
            var result = _campaignSceneResultPipeline.Get();
            Model.AddPoliticalActions(result.CampaignResult.RewardedPoliticalActions);
            result.CampaignResult.Dispose();
        }
    }
}