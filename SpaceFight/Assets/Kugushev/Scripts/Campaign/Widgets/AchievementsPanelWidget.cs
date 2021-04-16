using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class AchievementsPanelWidget : MonoBehaviour
    {
        [SerializeField] private CampaignModelProvider? modelProvider;
        [SerializeField] private Transform? achievementsPanel;
        [SerializeField] private GameObject? achievementCardPrefab;

        private void Start()
        {
            Asserting.NotNull(modelProvider);

            if (modelProvider.TryGetModel(out var model))
            {
                foreach (var achievement in model.PlayerPerks.CommonPerks)
                    CreateAchievementCard(achievement);

                foreach (var achievement in model.PlayerPerks.EpicPerks.Values)
                    CreateAchievementCard(achievement);
            }
        }

        private void CreateAchievementCard(PerkInfo perk)
        {
            Asserting.NotNull(achievementCardPrefab, achievementsPanel);

            var go = Instantiate(achievementCardPrefab, achievementsPanel);
            var widget = go.GetComponent<AchievementCardWidget>();
            widget.SetUp(perk);
        }
    }
}