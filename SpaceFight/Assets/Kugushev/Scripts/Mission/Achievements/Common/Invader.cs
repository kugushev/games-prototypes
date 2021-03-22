using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Common
{
    [CreateAssetMenu(menuName = MenuName + nameof(Invader))]
    public class Invader : AbstractAchievement, IPercentPerk<Army>
    {
        [SerializeField] private int percent = 10;

        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Invader, null, AchievementType.Common, nameof(Invader),
            "Invade to a planet", $"Increased siege damage on {percent}%");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            MissionModel model)
        {
            foreach (var missionEvent in missionEvents.PlanetCaptured)
                if (missionEvent.NewOwner == faction)
                    return true;
            return false;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
            => fleetPerks.siegeDamage.AddPerk(this);

        public Percentage GetPercentage(Army criteria) => new Percentage(percent);
    }
}