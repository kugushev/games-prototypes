using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Negotiator))]
    public class Negotiator : EpicPerk
    {
        [SerializeField] private float predominance;
        [SerializeField] [Range(0f, 1f)] private float surrendered;

        protected override PerkId PerkId => PerkId.Negotiator;

        protected override string Name => nameof(Negotiator);

        protected override string Criteria => $"Capture a neutral planet with {predominance} power predominance";

        protected override string Perk =>
            $"If you attack a neutral planet with {predominance} predominance, the planet surrender you. {surrendered} of the planet army joins you";

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            foreach (var planetCaptured in missionEvents.PlanetCaptured)
                if (planetCaptured.NewOwner == faction &&
                    planetCaptured.PreviousOwner == Faction.Neutral &&
                    planetCaptured.Overpower >= predominance)
                    return true;

            return false;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            if (fleetPerks.ToNeutralPlanetUltimatum.Initialized)
                Debug.LogError($"{fleetPerks.ToNeutralPlanetUltimatum} is already specified");

            fleetPerks.ToNeutralPlanetUltimatum = new SiegeUltimatum(new Percentage(surrendered), predominance);
        }
    }
}