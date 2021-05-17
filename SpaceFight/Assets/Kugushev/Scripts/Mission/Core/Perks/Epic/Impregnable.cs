using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Core.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Impregnable))]
    public class Impregnable : EpicPerk, IMultiplierPerk<Planet>
    {
        [SerializeField] private float totalPower;
        [SerializeField] private float powerCap;
        [SerializeField] private float multiplier;

        protected override PerkId PerkId => PerkId.Impregnable;
        protected override string Name => nameof(Impregnable);

        protected override string Criteria => $"Successfully defend against a siege with total power {totalPower}";

        protected override string Perk => $"Planets with more than {powerCap} power do x{multiplier} damage";

        public override bool Check(EventsCollectingService missionEvents, Faction faction)
        {
            float total = 0;
            foreach (var armyDestroyedOnSiege in missionEvents.ArmyDestroyedOnSiege)
            {
                if (armyDestroyedOnSiege.Destroyer == faction)
                    total += armyDestroyedOnSiege.ArmyStartPower;
            }

            return total >= totalPower;
        }

        public override void Apply(FleetEffects fleetEffects, PlanetarySystemEffects planetarySystemEffects)
        {
            planetarySystemEffects.Damage.AddPerk(this);
        }

        public float? GetMultiplier(Planet criteria)
        {
            if (criteria.Power.Value >= powerCap)
                return multiplier;

            return null;
        }
    }
}