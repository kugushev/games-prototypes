using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
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

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            int cnt = 0;
            foreach (var armySent in missionEvents.ArmySent)
                if (armySent.Owner == faction && armySent.Power >= powerCap)
                    cnt++;

            return cnt > count;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            fleetPerks.fightDamage.AddPerk(this);
            fleetPerks.siegeDamage.AddPerk(this);
        }

        public float? GetMultiplier(Army criteria)
        {
            if (criteria.Power >= powerCap)
                return multiplication;
            return null;
        }
    }
}