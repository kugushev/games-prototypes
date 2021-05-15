using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.Services;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    internal class CampaignDataInitializer : IInitializable, ITransition
    {
        private readonly ParametersPipeline<CampaignParameters> _parametersPipeline;
        private readonly CampaignMissions _campaignMissions;
        private readonly MissionsGenerationService _missionsGenerationService;

        internal CampaignDataInitializer(ParametersPipeline<CampaignParameters> parametersPipeline,
            CampaignMissions campaignMissions,
            MissionsGenerationService missionsGenerationService)
        {
            _parametersPipeline = parametersPipeline;
            _campaignMissions = campaignMissions;
            _missionsGenerationService = missionsGenerationService;
        }

        public bool ToTransition { get; private set; }

        public void Initialize()
        {
            var parameters = _parametersPipeline.Pop();
            
            Random.InitState(parameters.Seed);
            
            var missions = _missionsGenerationService.GenerateMissions();
            _campaignMissions.Init(missions, parameters.Budget);

            ToTransition = true;
        }
    }
}