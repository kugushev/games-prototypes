using System;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class CampaignProgressWidget : MonoBehaviour
    {
        [SerializeField] private CampaignModelProvider modelProvider;
        [SerializeField] private TextMeshProUGUI playerScoreText;
        [SerializeField] private TextMeshProUGUI aiScoreText;
        [SerializeField] private TextMeshProUGUI seedValueText;
        [SerializeField] private Slider seedSlider;

        [Header("Achievements Panel")] [SerializeField]
        private Transform achievementsPanel;

        [SerializeField] private GameObject achievementCardPrefab;

        private void Start()
        {
            if (modelProvider.TryGetModel(out var model))
            {
                playerScoreText.text = StringBag.FromInt(model.PlayerScore);
                aiScoreText.text = StringBag.FromInt(model.AIScore);
                seedValueText.text = StringBag.FromInt(model.NextMissionSeed);
                seedSlider.value = model.NextMissionSeed;

                foreach (var achievement in model.PlayerAchievements)
                {
                    var go = Instantiate(achievementCardPrefab, achievementsPanel);
                    var widget = go.GetComponent<AchievementCardWidget>();
                    widget.SetUp(achievement);
                }
            }
        }

        public void SetSeed(float sliderValue)
        {
            int value = Convert.ToInt32(sliderValue);
            seedValueText.text = StringBag.FromInt(value);
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionSeed = value;
        }
    }
}