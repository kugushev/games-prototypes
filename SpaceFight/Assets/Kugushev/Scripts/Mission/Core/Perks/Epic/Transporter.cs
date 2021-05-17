using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Transporter))]
    public class Transporter : EpicPerk, IMultiplierPerk<(Planet target, Faction playerFaction)>
    {
        [SerializeField] private float totalPower;
        [SerializeField] private float acceleration;

        protected override PerkId PerkId => PerkId.Transporter;
        protected override string Name => nameof(Transporter);

        protected override string Criteria => $"Transfer {totalPower} total power between your planets";

        protected override string Perk =>
            $"Increase armies speed in {acceleration} times of traveling between your planets";

        public override bool Check(EventsCollectingService missionEvents, Faction faction)
        {
            float total = 0;
            foreach (var armyArrived in missionEvents.ArmyArrived)
            {
                if (armyArrived.Owner == faction)
                    total += armyArrived.Power;
            }

            return total >= totalPower;
        }

        public override void Apply(FleetEffects fleetEffects, PlanetarySystemEffects planetarySystemEffects)
        {
            fleetEffects.ArmySpeed.AddPerk(this);
        }

        public float? GetMultiplier((Planet target, Faction playerFaction) criteria)
        {
            if (criteria.target.Faction.Value == criteria.playerFaction)
                return acceleration;
            return null;
        }
    }
}