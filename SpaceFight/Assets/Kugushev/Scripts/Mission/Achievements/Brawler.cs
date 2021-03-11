using System.Collections.Generic;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements
{
    [CreateAssetMenu(menuName = MenuName + nameof(Brawler))]
    public class Brawler : AbstractAchievement
    {
        public override AchievementInfo Info { get; } = new AchievementInfo(
            AchievementId.Brawler, null, AchievementType.Common, nameof(Brawler),
            "Destroy enemy army by your army. Bonus: Increased army to army damage");

        public override bool Check(IReadOnlyList<MissionEvent> missionEvents, Faction faction)
        {
            foreach (var missionEvent in missionEvents)
                if (missionEvent.EventType == MissionEventType.ArmyDestroyedInFight && missionEvent.Active == faction)
                    return true;
            return false;
        }

        public override void Apply(ref FleetPropertiesBuilder fleetProperties)
        {
            fleetProperties.FightMultiplier += 0.1f;
        }
    }
}