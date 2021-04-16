using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Managers
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(PerksManager))]
    public class PerksManager : ScriptableObject
    {
        [SerializeField] private MissionEventsCollector? eventsCollector;
        [SerializeField] private BasePerk[]? perks;
        [SerializeField] private MissionModelProvider? modelProvider;

        public void FindAchieved(List<BasePerk> listToFill, Faction playerFaction,
            PlayerPerks playerPerks)
        {
            Asserting.NotNull(modelProvider, perks, eventsCollector);

            if (!modelProvider.TryGetModel(out var model))
            {
                Debug.LogError("Unable to get model");
                return;
            }

            foreach (var achievement in perks)
            {
                if (!IsAllowedToCheck(playerPerks, achievement))
                    continue;


                if (achievement.Check(eventsCollector, playerFaction, model))
                    listToFill.Add(achievement);
            }
        }

        private static bool IsAllowedToCheck(PlayerPerks playerPerks, BasePerk perk)
        {
            switch (perk.Info.Type)
            {
                case PerkType.Common:
                    return true;
                case PerkType.Epic:
                    if (!playerPerks.AvailablePerks.Contains(perk.Info.Id))
                        return false;

                    if (perk.Info.Level != null)
                    {
                        var expectedLevel = GetExpectedLevel(playerPerks, perk);
                        return perk.Info.Level == expectedLevel;
                    }
                    else
                    {
                        Debug.LogError($"Epic achievement with null level {perk.Info}");
                        return false;
                    }
                default:
                {
                    Debug.LogError($"Unexpected achievement type {perk.Info}");
                    return false;
                }
            }
        }

        private static int GetExpectedLevel(PlayerPerks playerPerks, BasePerk perk)
        {
            if (playerPerks.EpicPerks.TryGetValue(perk.Info.Id, out var achieved))
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

        public void FindMatched(List<BasePerk> listToFill, PlayerPerks playerPerks)
        {
            Asserting.NotNull(perks);

            foreach (var achievement in perks)
            {
                if (playerPerks.EpicPerks.TryGetValue(achievement.Info.Id, out var epicAchievement)
                    && epicAchievement == achievement.Info)
                {
                    listToFill.Add(achievement);
                    continue;
                }

                foreach (var commonAchievement in playerPerks.CommonPerks)
                    if (commonAchievement == achievement.Info)
                        listToFill.Add(achievement);
            }
        }
    }
}