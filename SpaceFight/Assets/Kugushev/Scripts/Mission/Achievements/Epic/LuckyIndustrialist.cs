using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(LuckyIndustrialist))]
    public class LuckyIndustrialist : EpicAchievement
    {
        private const float ProbabilityMin = 0f;
        private const float ProbabilityMax = 1f;

        [SerializeField] private int count;
        [SerializeField] private float power;

        [SerializeField] [Range(ProbabilityMin, ProbabilityMax)]
        private float probability;

        protected override AchievementId AchievementId => AchievementId.LuckyIndustrialist;
        
        protected override string Name => "Lucky Industrialist";

        protected override string Criteria =>
            $"Recruit {count} armies with at least {power} power. Planets should be left empty.";

        protected override string Perk =>
            $"Armies with more than {power} power may be produced for free with probability {probability}";

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            int cnt = 0;
            foreach (var armySent in missionEvents.ArmySent)
                if (armySent.Owner == faction && armySent.Power >= power && armySent.RemainingPower <= 1f)
                    cnt++;

            return cnt >= count;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            if (planetarySystemPerks.IsFreeRecruitment != null)
                Debug.LogError($"Field {planetarySystemPerks.IsFreeRecruitment} has already specified");

            planetarySystemPerks.IsFreeRecruitment = IsFreeRecruitment;
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