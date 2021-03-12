using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Managers
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(AchievementsManager))]
    public class AchievementsManager : ScriptableObject
    {
        [SerializeField] private MissionEventsCollector eventsCollector;
        [SerializeField] private AbstractAchievement[] achievements;
        [SerializeField] private MissionModelProvider modelProvider;

        public void FindAchieved(List<AbstractAchievement> listToFill, Faction playerFaction,
            PlayerAchievements playerAchievements)
        {
            modelProvider.TryGetModel(out var model);

            foreach (var achievement in achievements)
            {
                if (!IsAllowedToCheck(playerAchievements, achievement))
                    continue;


                if (achievement.Check(eventsCollector, playerFaction, model))
                    listToFill.Add(achievement);
            }
        }

        private static bool IsAllowedToCheck(PlayerAchievements playerAchievements, AbstractAchievement achievement)
        {
            switch (achievement.Info.Type)
            {
                case AchievementType.Common:
                    return true;
                case AchievementType.Epic:
                    if (achievement.Info.Level != null)
                    {
                        var expectedLevel = GetExpectedLevel(playerAchievements, achievement);
                        return achievement.Info.Level == expectedLevel;
                    }
                    else
                    {
                        Debug.LogError($"Epic achievement with null level {achievement.Info}");
                        return false;
                    }
                default:
                {
                    Debug.LogError($"Unexpected achievement type {achievement.Info}");
                    return false;
                }
            }
        }

        private static int GetExpectedLevel(PlayerAchievements playerAchievements, AbstractAchievement achievement)
        {
            if (playerAchievements.EpicAchievements.TryGetValue(achievement.Info.Id, out var achieved))
            {
                if (achieved.Level == null)
                {
                    Debug.LogError($"Epic achievement with null level in the list {achieved}");
                    return 1;
                }

                return achieved.Level.Value + 1;
            }

            return 1;
        }

        public void FindMatched(List<AbstractAchievement> listToFill, PlayerAchievements playerAchievements)
        {
            foreach (var achievement in achievements)
            {
                if (playerAchievements.EpicAchievements.TryGetValue(achievement.Info.Id, out var epicAchievement)
                    && epicAchievement == achievement.Info)
                {
                    listToFill.Add(achievement);
                    continue;
                }

                foreach (var commonAchievement in playerAchievements.CommonAchievements)
                    if (commonAchievement == achievement.Info)
                        listToFill.Add(achievement);
            }
        }
    }
}