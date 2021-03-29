using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    // todo: add pooling
    [Serializable]
    public class PlayerAchievements
    {
        [SerializeReference] private List<PerkInfo> commonAchievements = new List<PerkInfo>();

        // stop thinking that using struct in dictionary cause boxing. It's not true right now, just turn on profiler and check
        [SerializeReference] private Dictionary<PerkId, PerkInfo> epicAchievements =
            new Dictionary<PerkId, PerkInfo>();

        public IReadOnlyList<PerkInfo> CommonAchievements => commonAchievements;
        public IReadOnlyDictionary<PerkId, PerkInfo> EpicAchievements => epicAchievements;

        internal void AddAchievement(PerkInfo perkInfo)
        {
            switch (perkInfo.Type)
            {
                case PerkType.Common:
                    AddCommon(perkInfo);
                    break;
                case PerkType.Epic:
                    AddEpic(perkInfo);
                    break;
                default:
                    Debug.LogError($"Unexpected achievement type {perkInfo}");
                    break;
            }
        }

        private void AddCommon(PerkInfo perkInfo)
        {
            if (perkInfo.Level != null)
                Debug.LogError($"Common achievement with non null level: {perkInfo}");

            commonAchievements.Add(perkInfo);
        }

        private void AddEpic(PerkInfo perkInfo)
        {
            if (epicAchievements.TryGetValue(perkInfo.Id, out var current) &&
                (current.Level >= perkInfo.Level || perkInfo.Level == null || current.Level == null))
            {
                Debug.LogError($"Unexpected achievement. Current {current}, new {perkInfo}");
                return;
            }

            epicAchievements[perkInfo.Id] = perkInfo;
        }
    }
}