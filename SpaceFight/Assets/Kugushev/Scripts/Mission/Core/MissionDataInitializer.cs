using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Services;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionDataInitializer : IInitializable, ITransition
    {
        private readonly ParametersPipeline<MissionParameters> _parametersPipeline;
        private readonly PlanetarySystem _planetarySystem;
        private readonly PlanetarySystemGenerationService _planetarySystemGenerationService;
        private readonly EffectsService _effectsService;

        public MissionDataInitializer(ParametersPipeline<MissionParameters> parametersPipeline,
            PlanetarySystem planetarySystem,
            PlanetarySystemGenerationService planetarySystemGenerationService,
            EffectsService effectsService)
        {
            _parametersPipeline = parametersPipeline;
            _planetarySystem = planetarySystem;
            _planetarySystemGenerationService = planetarySystemGenerationService;
            _effectsService = effectsService;
        }

        public bool ToTransition { get; private set; }

        public void Initialize()
        {
            var parameters = _parametersPipeline.Pop();

            var (planetarySystemEffects, fleetEffects) =
                _effectsService.ComposeEffects(_planetarySystemGenerationService.PlayerFaction);


            var (sun, planets) = _planetarySystemGenerationService.CreatePlanetarySystemData(parameters.MissionInfo,
                planetarySystemEffects);
            _planetarySystem.Init(sun, planets);

            ToTransition = true;
        }
    }
}