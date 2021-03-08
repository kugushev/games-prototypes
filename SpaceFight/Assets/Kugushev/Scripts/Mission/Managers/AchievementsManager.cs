using System.Collections.Generic;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Managers
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(AchievementsManager))]
    public class AchievementsManager : ScriptableObject
    {
        [SerializeField] private MissionEventsCollector eventsCollector;
        [SerializeField] private AbstractAchievement[] achievements;

        public void FindAchieved(List<AbstractAchievement> listToFill, Faction playerFaction)
        {
            foreach (var achievement in achievements)
                if (achievement.Check(eventsCollector.Events, playerFaction))
                    listToFill.Add(achievement);
        }

        public void FindMatched(List<AbstractAchievement> listToFill, IReadOnlyList<AchievementInfo> achievementsInfo)
        {
            foreach (var achievementInfo in achievementsInfo)
            foreach (var achievement in achievements)
                if (achievement.Info == achievementInfo)
                    listToFill.Add(achievement);
        }
    }
}