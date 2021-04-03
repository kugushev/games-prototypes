using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class AchievementsPanelWidget : MonoBehaviour
    {
        [SerializeField] private CampaignModelProvider modelProvider;
        [SerializeField] private Transform achievementsPanel;
        [SerializeField] private GameObject achievementCardPrefab;

        private void Start()
        {
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
            var go = Instantiate(achievementCardPrefab, achievementsPanel);
            var widget = go.GetComponent<AchievementCardWidget>();
            widget.SetUp(perk);
        }
    }
}