using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Core.Services
{
    public class EffectsService
    {
        private readonly IPlayerPerks _playerPerks;
        private readonly PerksSearchService _perksSearchService;


        public EffectsService(IPlayerPerks playerPerks, PerksSearchService perksSearchService)
        {
            _playerPerks = playerPerks;
            _perksSearchService = perksSearchService;
        }

        public (IPlanetarySystemEffects, IFleetEffects) ComposeEffects(Faction playerFaction)
        {
            var perks = _perksSearchService.FindMatched(_playerPerks);

            var fleetEffects = new FleetEffects();
            var planetarySystemEffects = new PlanetarySystemEffects(playerFaction);

            foreach (var perk in perks)
                perk.Apply(fleetEffects, planetarySystemEffects);

            return (planetarySystemEffects, fleetEffects);
        }
    }
}