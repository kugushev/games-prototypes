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
    [CreateAssetMenu(menuName = MenuName + nameof(Moska))]
    public class Moska : BasePerk, IMultiplierPerk<Army>
    {
        [SerializeField] private int level;
        [SerializeField] private float powerCap;
        [SerializeField] private float multiplication;

        private PerkInfo? _info;

        public override PerkInfo Info => _info ??= new PerkInfo(
            PerkId.Moska, level, PerkType.Epic, nameof(Moska),
            $"Have armies only with no more {powerCap} power",
            $"Armies with less than {powerCap} power receive and deal only {multiplication} of the damage on fights");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmySent)
                if (missionEvent.Owner == faction && missionEvent.Power > powerCap)
                    return false;

            return true;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            fleetPerks.fightDamage.AddPerk(this);
            fleetPerks.fightProtection.AddPerk(this);
        }

        public float? GetMultiplier(Army criteria)
        {
            if (criteria.Power <= powerCap)
                return multiplication;
            return null;
        }
    }
}