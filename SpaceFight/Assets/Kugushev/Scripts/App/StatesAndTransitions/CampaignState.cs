﻿using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.App.StatesAndTransitions
{
    internal class CampaignState : BaseSceneLoadingState<AppModel>
    {
        private readonly CampaignSceneParametersPipeline _sceneParametersPipeline;
        private readonly bool _isPlayground;

        public CampaignState(AppModel model, CampaignSceneParametersPipeline sceneParametersPipeline,
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
            var campaignInfo = new CampaignInfo(Model.MainMenu.Seed, null, _isPlayground);
            _sceneParametersPipeline.Set(campaignInfo);
        }
    }
}