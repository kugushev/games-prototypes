using System.Collections.Generic;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements
{
    [CreateAssetMenu(menuName = MenuName + "Invader")]
    public class Invader : AbstractAchievement
    {
        [SerializeField] private int level;

        private AchievementInfo? _info;

        public override AchievementInfo Info => _info ??= new AchievementInfo(
            AchievementId.Invader,
            level,
            nameof(Invader),
            "Invade to a planet");

        public override bool Check(IReadOnlyList<MissionEvent> missionEvents, Faction faction)
        {
            foreach (var missionEvent in missionEvents)
                if (missionEvent.EventType == MissionEventType.PlanetCaptured && missionEvent.Active == faction)
                    return true;
            return false;
        }
    }
}