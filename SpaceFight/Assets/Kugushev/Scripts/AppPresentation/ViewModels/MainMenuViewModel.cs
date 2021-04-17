using System;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            // _serviceBus.Pub(new NewGameSignal
            // {
            //     Seed = seed
            // });
        }

        private void OnCustomCampaignClicked()
        {
            // _serviceBus.Pub(new StartCampaignSignal
            // {
            //     Seed = seed,
            //     Perks = null
            // });
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