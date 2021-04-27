using System;
using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.AppPresentation.ViewModels
{
    internal class MainMenuViewModel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? seedValueText;
        [SerializeField] private Slider? seedSlider;
        [SerializeField] private Button? newGameButton;
        [SerializeField] private Button? customCampaignButton;
        [SerializeField] private Button? playgroundButton;

        [SerializeField] private int seed = AppConstants.DefaultSeed;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private SignalToTransition<GameParameters>.Pool _newGamePool = default!;

        private void Start()
        {
            Asserting.NotNull(seedSlider, seedValueText, newGameButton, customCampaignButton,
                playgroundButton);

            newGameButton.onClick.AddListener(OnNewGameClicked);
            customCampaignButton.onClick.AddListener(OnCustomCampaignClicked);
            playgroundButton.onClick.AddListener(OnPlaygroundClicked);
            seedSlider.onValueChanged.AddListener(OnSeedValueChanged);

            seedSlider.value = seed;
            seedValueText.text = StringBag.FromInt(seed);
        }

        private void OnNewGameClicked()
        {
            _signalBus.Fire(_newGamePool.Spawn(new GameParameters(seed)));
        }

        private void OnCustomCampaignClicked()
        {
            // todo: start campaign
        }

        private void OnPlaygroundClicked()
        {
            // todo: start playground
        }

        private void OnSeedValueChanged(float sliderValue)
        {
            Asserting.NotNull(seedValueText);

            seed = Convert.ToInt32(sliderValue);
            seedValueText.text = StringBag.FromInt(seed);
        }
    }
}