using System.Collections.Generic;
using Kugushev.Scripts.Game.Missions.Achievements.Abstractions;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Achievements
{
    [CreateAssetMenu(menuName = MenuName + "Invader")]
    public class Invader : AbstractAchievement
    {
        [SerializeField] private int level;

        public override AchievementId Id => AchievementId.Invader;
        public override int Level => level;
        public override string Caption => nameof(Invader);
        public override string Description => "Invade to a planet";

        public override bool Check(IReadOnlyList<MissionEvent> missionEvents, Faction faction)
        {
            foreach (var missionEvent in missionEvents)
                if (missionEvent.EventType == MissionEventType.PlanetCaptured && missionEvent.Active == faction)
                    return true;
            return false;
        }
    }
}