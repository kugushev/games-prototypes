using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Utils;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    internal class CampaignState : BaseSceneLoadingState<GameModel>
    {
        private readonly CampaignSceneParametersPipeline _sceneParametersPipeline;

        public CampaignState(GameModel model, CampaignSceneParametersPipeline sceneParametersPipeline)
            : base(model, UnityConstants.CampaignManagementScene, false)
        {
            _sceneParametersPipeline = sceneParametersPipeline;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var campaignInfo = new CampaignInfo(Model.MainMenu.Seed);
            _sceneParametersPipeline.Set(campaignInfo);
        }
    }
}