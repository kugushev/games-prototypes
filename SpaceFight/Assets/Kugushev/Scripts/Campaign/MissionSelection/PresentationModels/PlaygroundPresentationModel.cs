using Kugushev.Scripts.Common.Widgets;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.MissionSelection.PresentationModels
{
    public class PlaygroundPresentationModel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? playerScoreText;
        [SerializeField] private TextMeshProUGUI? aiScoreText;
        [SerializeField] private PanelWithSliderWidget? seed;
        [SerializeField] private PanelWithSliderWidget? playerHomeProduction;
        [SerializeField] private PanelWithSliderWidget? enemyHomeProduction;
        [SerializeField] private PanelWithSliderWidget? playerExtraPlanets;
        [SerializeField] private PanelWithSliderWidget? enemyExtraPlanets;
        [SerializeField] private PanelWithSliderWidget? playerStartPower;
        [SerializeField] private PanelWithSliderWidget? enemyStartPower;

        private struct Model
        {
            public int Seed;
            public int? PlayerHomeProductionMultiplier;
            public int? EnemyHomeProductionMultiplier;
            public int? PlayerExtraPlanets;
            public int? EnemyExtraPlanets;
            public int? PlayerStartPowerMultiplier;
            public int? EnemyStartPowerMultiplier;
        }

        private Model _model = new Model();


        public void SetSeed(float sliderValue)
        {
            int value = Mathf.FloorToInt(sliderValue);

            _model.Seed = value;
        }

        public void SetPlayerHomeProductionMultiplier(float sliderValue)
        {
            _model.PlayerHomeProductionMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyHomeProductionMultiplier(float sliderValue)
        {
            _model.EnemyHomeProductionMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetPlayerExtraPlanets(float sliderValue)
        {
            _model.PlayerExtraPlanets = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyExtraPlanets(float sliderValue)
        {
            _model.EnemyExtraPlanets = Mathf.FloorToInt(sliderValue);
        }

        public void SetPlayerStartPower(float sliderValue)
        {
            _model.PlayerStartPowerMultiplier = Mathf.FloorToInt(sliderValue);
        }

        public void SetEnemyStartPower(float sliderValue)
        {
            _model.EnemyStartPowerMultiplier = Mathf.FloorToInt(sliderValue);
        }
    }
}