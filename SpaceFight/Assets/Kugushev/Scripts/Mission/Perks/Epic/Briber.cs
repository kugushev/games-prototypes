using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Briber))]
    public class Briber : EpicPerk
    {
        private const float ProbabilityMin = 0f;
        private const float ProbabilityMax = 1f;
        [SerializeField] private float captureTimeInSeconds;

        [SerializeField] [Range(ProbabilityMin, ProbabilityMax)]
        private float probability;

        protected override PerkId PerkId => PerkId.Briber;
        protected override string Name => nameof(Briber);
        protected override string Criteria => $"Capture neutral planet in {captureTimeInSeconds} seconds after start";

        protected override string Perk =>
            $"With a probability {probability} one of the neutral planets become your at the beginning of any mission";

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            foreach (var planetCaptured in missionEvents.PlanetCaptured)
            {
                if (planetCaptured.NewOwner == faction &&
                    planetCaptured.PreviousOwner == Faction.Neutral &&
                    planetCaptured.Time.TotalSeconds <= captureTimeInSeconds)
                    return true;
            }

            return false;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            if (planetarySystemPerks.GetExtraPlanetOnStart != null)
                Debug.LogError($"{planetarySystemPerks.GetExtraPlanetOnStart} is already specified");
            planetarySystemPerks.GetExtraPlanetOnStart = GetExtraPlanetOnStart;
        }

        private bool GetExtraPlanetOnStart()
        {
            var range = SubstitutiveRandom.Range(ProbabilityMin, ProbabilityMax);
            return range <= probability;
        }
    }
}