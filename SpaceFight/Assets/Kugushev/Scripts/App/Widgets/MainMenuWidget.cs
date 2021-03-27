using System;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.App.Widgets
{
    internal class MainMenuWidget : MonoBehaviour
    {
        [SerializeField] private GameModelProvider gameModelProvider;
        [SerializeField] private TextMeshProUGUI seedValueText;
        [SerializeField] private Slider seedSlider;

        private void Start()
        {
            if (gameModelProvider.TryGetModel(out var gameModel))
            {
                var seed = gameModel.MainMenu.Seed;
                seedSlider.value = seed;
                seedValueText.text = StringBag.FromInt(seed);
            }
        }

        public void SetSeed(float sliderValue)
        {
            int value = Convert.ToInt32(sliderValue);
            seedValueText.text = StringBag.FromInt(value);
            if (gameModelProvider.TryGetModel(out var gameModel)) 
                gameModel.MainMenu.Seed = value;
        }
    }
}