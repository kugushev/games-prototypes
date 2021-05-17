using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Common
{
    [CreateAssetMenu(menuName = MenuName + nameof(Invader))]
    public class Invader : BasePerk, IPercentPerk<Army>
    {
        [SerializeField] private int percent = 10;

        private PerkInfo? _info;

        public override PerkInfo Info => _info ??= new PerkInfo(
            PerkId.Invader, null, PerkType.Common, nameof(Invader),
            "Invade to a planet", $"Increased siege damage on {percent}%");

        public override bool Check(EventsCollectingService missionEvents, Faction faction)
        {
            foreach (var missionEvent in missionEvents.PlanetCaptured)
                if (missionEvent.NewOwner == faction)
                    return true;
            return false;
        }

        public override void Apply(FleetEffects fleetEffects, PlanetarySystemEffects planetarySystemEffects)
            => fleetEffects.SiegeDamage.AddPerk(this);

        public Percentage GetPercentage(Army criteria) => new Percentage(percent);
    }
}