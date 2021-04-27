using System;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.AppPresentation.Signals;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.AppPresentation.ViewModels
{
    internal class MainMenuViewModel : MonoBehaviour
    {
        // injected
        // private dynamic _serviceBus;

        [SerializeField] private TextMeshProUGUI? seedValueText;
        [SerializeField] private Slider? seedSlider;
        [SerializeField] private Button? newGameButton;
        [SerializeField] private Button? customCampaignButton;
        [SerializeField] private Button? playgroundButton;

        [SerializeField] private int seed = 42;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private NewGameSignal.Pool _newGameSignalPool = default!;

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
            _signalBus.Fire(_newGameSignalPool.Spawn(new GameModeParameters(seed)));
        }

        private void OnCustomCampaignClicked()
        {
            // todo: start campaign
        }

        private void OnPlaygroundClicked()
        {
            // _serviceBus.Pub(new StartMissionSignal
            // {
            //     Seed = seed,
            //     Perks = null,
            //     MissionInfo = null
            // });
        }

        private void OnSeedValueChanged(float sliderValue)
        {
            Asserting.NotNull(seedValueText);

            seed = Convert.ToInt32(sliderValue);
            seedValueText.text = StringBag.FromInt(seed);
        }
    }
}