using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(LuckyIndustrialist))]
    public class LuckyIndustrialist : EpicPerk
    {
        private const float ProbabilityMin = 0f;
        private const float ProbabilityMax = 1f;

        [SerializeField] private int count;
        [SerializeField] private float power;

        [SerializeField] [Range(ProbabilityMin, ProbabilityMax)]
        private float probability;

        protected override PerkId PerkId => PerkId.LuckyIndustrialist;
        
        protected override string Name => "Lucky Industrialist";

        protected override string Criteria =>
            $"Recruit {count} armies with at least {power} power. Planets should be left empty.";

        protected override string Perk =>
            $"Armies with more than {power} power may be produced for free with probability {probability}";

        public override bool Check(EventsCollectingService missionEvents, Faction faction)
        {
            int cnt = 0;
            foreach (var armySent in missionEvents.ArmySent)
                if (armySent.Owner == faction && armySent.Power >= power && armySent.RemainingPower <= 1f)
                    cnt++;

            return cnt >= count;
        }

        public override void Apply(FleetEffects fleetEffects, PlanetarySystemEffects planetarySystemEffects)
        {
            if (planetarySystemEffects.IsFreeRecruitment != null)
                Debug.LogError($"Field {planetarySystemEffects.IsFreeRecruitment} has already specified");

            planetarySystemEffects.IsFreeRecruitment = IsFreeRecruitment;
        }

        private bool IsFreeRecruitment(float powerToRecruit)
        {
            if (powerToRecruit >= power)
            {
                var range = SubstitutiveRandom.Range(ProbabilityMin, ProbabilityMax);
                return range <= probability;
            }

            return false;
        }
    }
}