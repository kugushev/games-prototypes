using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Widgets;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class PlaygroundWidget : MonoBehaviour
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

        private void Start()
        {
            if (modelProvider.TryGetModel(out var rootModel))
            {
                var model = rootModel.Playground;
                playerScoreText.text = StringBag.FromInt(model.PlayerScore);
                aiScoreText.text = StringBag.FromInt(model.AIScore);

                seed.Value = model.Seed;
                playerHomeProduction.Value = model.PlayerHomeProductionMultiplier ?? 1;
                enemyHomeProduction.Value = model.EnemyHomeProductionMultiplier ?? 1;
                playerExtraPlanets.Value = model.PlayerExtraPlanets ?? 0;
                enemyExtraPlanets.Value = model.EnemyExtraPlanets ?? 0;
                playerStartPower.Value = model.PlayerStartPowerMultiplier ?? 0;
                enemyStartPower.Value = model.EnemyStartPowerMultiplier ?? 0;
            }
        }

        public void SetSeed(float sliderValue)
        {
            int value = Mathf.FloorToInt(sliderValue);
            if (modelProvider.TryGetModel(out var model))
                model.Playground.Seed = value;
        }

        public void SetPlayerHomeProductionMultiplier(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.Playground.PlayerHomeProductionMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyHomeProductionMultiplier(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.Playground.EnemyHomeProductionMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetPlayerExtraPlanets(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.Playground.PlayerExtraPlanets = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyExtraPlanets(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.Playground.EnemyExtraPlanets = Mathf.FloorToInt(sliderValue);
        }

        public void SetPlayerStartPower(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.Playground.PlayerStartPowerMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyStartPower(float sliderValue)
        {
            if (modelProvider.TryGetModel(out var model))
                model.Playground.EnemyStartPowerMultiplier = Mathf.FloorToInt(sliderValue);
        }
    }
}