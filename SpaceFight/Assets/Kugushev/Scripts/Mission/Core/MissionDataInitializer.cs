using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Enums;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionDataInitializer : IInitializable, ITransition
    {
        private readonly ParametersPipeline<MissionParameters> _parametersPipeline;
        private readonly PlanetarySystemSpecs _planetarySystemSpecs;
        private readonly PlanetarySystem _planetarySystem;
        private readonly PlanetarySystemGenerationService _planetarySystemGenerationService;
        private readonly EffectsService _effectsService;
        private readonly GreenFleet _greenFleet;
        private readonly RedFleet _redFleet;

        public MissionDataInitializer(ParametersPipeline<MissionParameters> parametersPipeline,
            PlanetarySystemSpecs planetarySystemSpecs,
            PlanetarySystem planetarySystem,
            PlanetarySystemGenerationService planetarySystemGenerationService,
            EffectsService effectsService,
            GreenFleet greenFleet,
            RedFleet redFleet)
        {
            _parametersPipeline = parametersPipeline;
            _planetarySystemSpecs = planetarySystemSpecs;
            _planetarySystem = planetarySystem;
            _planetarySystemGenerationService = planetarySystemGenerationService;
            _effectsService = effectsService;
            _greenFleet = greenFleet;
            _redFleet = redFleet;
        }

        public bool ToTransition { get; private set; }

        public void Initialize()
        {
            var parameters = _parametersPipeline.Pop();

            var (planetarySystemEffects, fleetEffects) =
                _effectsService.ComposeEffects(_planetarySystemSpecs.PlayerFaction);


            switch (_planetarySystemSpecs.PlayerFaction)
            {
                case Faction.Green:
                    _greenFleet.SetFleetEffects(fleetEffects);
                    break;
                case Faction.Red:
                    _redFleet.SetFleetEffects(fleetEffects);
                    break;
            }
            
            var (sun, planets) = _planetarySystemGenerationService.CreatePlanetarySystemData(parameters.MissionInfo);
            _planetarySystem.Init(sun, planets, planetarySystemEffects);

            ToTransition = true;
        }
    }
}