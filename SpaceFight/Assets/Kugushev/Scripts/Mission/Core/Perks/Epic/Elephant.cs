using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Elephant))]
    public class Elephant : BasePerk, IMultiplierPerk<Army>
    {
        [SerializeField] private int level;
        [SerializeField] private float count;
        [SerializeField] private float powerCap;
        [SerializeField] private float multiplication;

        private PerkInfo? _info;

        public override PerkInfo Info => _info ??= new PerkInfo(
            PerkId.Elephant, level, PerkType.Epic, nameof(Elephant),
            $"Have {count} armies with more {powerCap} power",
            $"Armies with more than {powerCap} power deal to {multiplication} more damage on fight and on siege");

        public override bool Check(EventsCollectingService missionEvents, Faction faction)
        {
            int cnt = 0;
            foreach (var armySent in missionEvents.ArmySent)
                if (armySent.Owner == faction && armySent.Power >= powerCap)
                    cnt++;

            return cnt > count;
        }

        public override void Apply(FleetEffects fleetEffects, PlanetarySystemEffects planetarySystemEffects)
        {
            fleetEffects.FightDamage.AddPerk(this);
            fleetEffects.SiegeDamage.AddPerk(this);
        }

        public float? GetMultiplier(Army criteria)
        {
            if (criteria.Power >= powerCap)
                return multiplication;
            return null;
        }
    }
}