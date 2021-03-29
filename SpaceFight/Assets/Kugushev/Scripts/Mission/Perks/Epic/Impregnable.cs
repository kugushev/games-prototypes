using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
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

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            float total = 0;
            foreach (var armyDestroyedOnSiege in missionEvents.ArmyDestroyedOnSiege)
            {
                if (armyDestroyedOnSiege.Destroyer == faction)
                    total += armyDestroyedOnSiege.ArmyStartPower;
            }

            return total >= totalPower;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            planetarySystemPerks.damage.AddPerk(this);
        }

        public float? GetMultiplier(Planet criteria)
        {
            if (criteria.Power >= powerCap)
                return multiplier;

            return null;
        }
    }
}