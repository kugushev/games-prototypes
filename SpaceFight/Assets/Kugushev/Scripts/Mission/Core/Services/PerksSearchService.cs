using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Core.Services
{
    public class PerksSearchService
    {
        private readonly PerksRegistry _perksRegistry;
        private readonly EventsCollectingService _eventsCollectingService;

        public PerksSearchService(PerksRegistry perksRegistry, EventsCollectingService eventsCollectingService)
        {
            _perksRegistry = perksRegistry;
            _eventsCollectingService = eventsCollectingService;
        }

        public void FindAchieved(List<BasePerk> listToFill, Faction playerFaction,
            IPlayerPerks playerPerks)
        {
            foreach (var perk in _perksRegistry.Perks)
            {
                if (!IsAllowedToCheck(playerPerks, perk))
                    continue;


                if (perk.Check(_eventsCollectingService, playerFaction))
                    listToFill.Add(perk);
            }
        }

        private static bool IsAllowedToCheck(IPlayerPerks playerPerks, BasePerk perk)
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

        private static int GetExpectedLevel(IPlayerPerks playerPerks, BasePerk perk)
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

        public IReadOnlyList<BasePerk> FindMatched(IPlayerPerks playerPerks)
        {
            var result = new List<BasePerk>();
            foreach (var achievement in _perksRegistry.Perks)
            {
                if (playerPerks.EpicPerks.TryGetValue(achievement.Info.Id, out var epicAchievement)
                    && epicAchievement == achievement.Info)
                {
                    result.Add(achievement);
                    continue;
                }

                foreach (var commonAchievement in playerPerks.CommonPerks)
                    if (commonAchievement == achievement.Info)
                        result.Add(achievement);
            }

            return result;
        }
    }
}