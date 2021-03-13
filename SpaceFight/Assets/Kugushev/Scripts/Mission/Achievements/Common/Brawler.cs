using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Common
{
    [CreateAssetMenu(menuName = MenuName + nameof(Brawler))]
    public class Brawler : AbstractAchievement
    {
        public override AchievementInfo Info { get; } = new AchievementInfo(
            AchievementId.Brawler, null, AchievementType.Common, nameof(Brawler),
            "Destroy enemy army by your army", "Increased army to army damage on 10%");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction,
            MissionModel model)
        {
            foreach (var missionEvent in missionEvents.ArmyDestroyedInFight)
                if (missionEvent.Destroyer == faction)
                    return true;
            return false;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties,
            ref PlanetarySystemPropertiesBuilder planetarySystemProperties)
        {
            fleetProperties.FightMultiplier += 0.1f;
        }
    }
}