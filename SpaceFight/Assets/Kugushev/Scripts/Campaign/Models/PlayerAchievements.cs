using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    // todo: add pooling
    [Serializable]
    public class PlayerAchievements
    {
        [SerializeReference] private List<AchievementInfo> commonAchievements = new List<AchievementInfo>();

        // stop thinking that using struct in dictionary cause boxing. It's not true right now, just turn on profiler and check
        [SerializeReference] private Dictionary<AchievementId, AchievementInfo> epicAchievements =
            new Dictionary<AchievementId, AchievementInfo>();

        public IReadOnlyList<AchievementInfo> CommonAchievements => commonAchievements;
        public IReadOnlyDictionary<AchievementId, AchievementInfo> EpicAchievements => epicAchievements;

        internal void AddAchievement(AchievementInfo achievementInfo)
        {
            switch (achievementInfo.Type)
            {
                case AchievementType.Common:
                    AddCommon(achievementInfo);
                    break;
                case AchievementType.Epic:
                    AddEpic(achievementInfo);
                    break;
                default:
                    Debug.LogError($"Unexpected achievement type {achievementInfo}");
                    break;
            }
        }

        private void AddCommon(AchievementInfo achievementInfo)
        {
            if (achievementInfo.Level != null)
                Debug.LogError($"Common achievement with non null level: {achievementInfo}");

            commonAchievements.Add(achievementInfo);
        }

        private void AddEpic(AchievementInfo achievementInfo)
        {
            if (epicAchievements.TryGetValue(achievementInfo.Id, out var current) &&
                (current.Level >= achievementInfo.Level || achievementInfo.Level == null || current.Level == null))
            {
                Debug.LogError($"Unexpected achievement. Current {current}, new {achievementInfo}");
                return;
            }

            epicAchievements[achievementInfo.Id] = achievementInfo;
        }
    }
}