using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Startup))]
    public class Startup : BasePerk, IMultiplierPerk<Planet>
    {
        [SerializeField] private int level;
        [SerializeField] private float maxPower;
        [SerializeField] private float multiplier;
        private PerkInfo? _info;

        public override PerkInfo Info => _info ??= new PerkInfo(
            PerkId.Startup, level, PerkType.Epic, nameof(Startup),
            $"Recruit only on planets that have less than {maxPower} power",
            $"Increase production to {multiplier} if power is less than {maxPower}, decreased if more");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmySent)
                if (missionEvent.Owner == faction && missionEvent.RemainingPower + missionEvent.Power > maxPower)
                    return false;

            return true;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
            => planetarySystemPerks.Production.AddPerk(this);

        public float? GetMultiplier(Planet criteria)
        {
            if (criteria.Power <= maxPower)
                return multiplier;
            return 1 / multiplier;
        }
    }
}