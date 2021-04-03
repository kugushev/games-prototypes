using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.App.StatesAndTransitions
{
    internal class CustomCampaignState : BaseSceneLoadingState<AppModel>
    {
        private readonly CampaignSceneParametersPipeline _sceneParametersPipeline;
        private readonly bool _isPlayground;

        public CustomCampaignState(AppModel model, CampaignSceneParametersPipeline sceneParametersPipeline,
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
            var campaignInfo = new CampaignInfo(Model.MainMenu.Seed, null, PerkIdHelper.AllPerks, _isPlayground);
            _sceneParametersPipeline.Set(campaignInfo);
        }
    }
}