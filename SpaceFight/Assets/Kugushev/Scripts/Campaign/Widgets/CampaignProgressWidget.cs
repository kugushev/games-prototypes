using System;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Common.Widgets;
using Kugushev.Scripts.Game.ValueObjects;
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
        [SerializeField] private PanelWithSliderWidget seed;
        [SerializeField] private PanelWithSliderWidget playerHomeProduction;
        [SerializeField] private PanelWithSliderWidget enemyHomeProduction;
        [SerializeField] private PanelWithSliderWidget playerExtraPlanets;
        [SerializeField] private PanelWithSliderWidget enemyExtraPlanets;
        [SerializeField] private PanelWithSliderWidget playerStartPower;
        [SerializeField] private PanelWithSliderWidget enemyStartPower;

        [Header("Achievements Panel")] [SerializeField]
        private Transform achievementsPanel;

        [SerializeField] private GameObject achievementCardPrefab;

        private void Start()
        {
            if (modelProvider.TryGetModel(out var model))
            {
                playerScoreText.text = StringBag.FromInt(model.PlayerScore);
                aiScoreText.text = StringBag.FromInt(model.AIScore);

                seed.Value = model.NextMissionProperties.Seed;
                playerHomeProduction.Value = model.NextMissionProperties.PlayerHomeProductionMultiplier ?? 1;
                enemyHomeProduction.Value = model.NextMissionProperties.EnemyHomeProductionMultiplier ?? 1;
                playerExtraPlanets.Value = model.NextMissionProperties.PlayerExtraPlanets ?? 0;
                enemyExtraPlanets.Value = model.NextMissionProperties.EnemyExtraPlanets ?? 0;
                playerStartPower.Value = model.NextMissionProperties.PlayerStartPower ?? 0;
                enemyStartPower.Value = model.NextMissionProperties.EnemyStartPower ?? 0;


                foreach (var achievement in model.PlayerAchievements.CommonAchievements)
                    CreateAchievementCard(achievement);

                foreach (var achievement in model.PlayerAchievements.EpicAchievements.Values)
                    CreateAchievementCard(achievement);
            }
        }

        private void CreateAchievementCard(AchievementInfo achievement)
        {
            var go = Instantiate(achievementCardPrefab, achievementsPanel);
            var widget = go.GetComponent<AchievementCardWidget>();
            widget.SetUp(achievement);
        }

        public void SetSeed(float sliderValue)
        {
            int value = Mathf.FloorToInt(sliderValue);
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.Seed = value;
        }

        public void SetPlayerHomeProductionMultiplier(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.PlayerHomeProductionMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyHomeProductionMultiplier(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.EnemyHomeProductionMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetPlayerExtraPlanets(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.PlayerExtraPlanets = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyExtraPlanets(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.EnemyExtraPlanets = Mathf.FloorToInt(sliderValue);
        }

        public void SetPlayerStartPower(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.PlayerStartPower = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyStartPower(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.NextMissionProperties.EnemyStartPower = Mathf.FloorToInt(sliderValue);
        }
    }
}