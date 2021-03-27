using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Common
{
    [CreateAssetMenu(menuName = MenuName + nameof(Brawler))]
    public class Brawler : AbstractAchievement, IPercentPerk<Army>
    {
        [SerializeField] private int percent = 10;

        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Brawler, null, AchievementType.Common, nameof(Brawler),
            "Destroy enemy army by your army", $"Increased army to army damage on {percent}%");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmyDestroyedInFight)
                if (missionEvent.Destroyer == faction)
                    return true;
            return false;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
            => fleetPerks.fightDamage.AddPerk(this);

        public Percentage GetPercentage(Army criteria) => new Percentage(percent);
    }
}