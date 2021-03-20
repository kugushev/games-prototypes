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
        private readonly bool _isPlayground;

        public CampaignState(GameModel model, CampaignSceneParametersPipeline sceneParametersPipeline,
            bool isPlayground)
            : base(model, UnityConstants.CampaignManagementScene, false)
        {
            _sceneParametersPipeline = sceneParametersPipeline;
            _isPlayground = isPlayground;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var campaignInfo = new CampaignInfo(Model.MainMenu.Seed, _isPlayground);
            _sceneParametersPipeline.Set(campaignInfo);
        }
    }
}