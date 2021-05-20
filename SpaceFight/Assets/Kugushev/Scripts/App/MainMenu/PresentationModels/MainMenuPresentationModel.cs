using System;
using Kugushev.Scripts.App.Core.Constants;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.App.MainMenu.PresentationModels
{
    internal class MainMenuPresentationModel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI seedValueText = default!;
        [SerializeField] private Slider seedSlider = default!;
        [SerializeField] private Button newGameButton = default!;
        [SerializeField] private Button customCampaignButton = default!;
        [SerializeField] private Button playgroundButton = default!;

        private readonly ReactiveProperty<int> _seed = new ReactiveProperty<int>();

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private SignalToTransition<GameParameters>.Factory _newGameSignalFactory = default!;

        private void Start()
        {
            newGameButton.onClick.AddListener(OnNewGameClicked);
            customCampaignButton.onClick.AddListener(OnCustomCampaignClicked);
            playgroundButton.onClick.AddListener(OnPlaygroundClicked);

            seedSlider.OnValueChangedAsObservable()
                .Select(Convert.ToInt32)
                .Subscribe(v => _seed.Value = v)
                .AddTo(this);
            _seed.Select(StringBag.FromInt)
                .SubscribeToTextMeshPro(seedValueText)
                .AddTo(this);
            seedSlider.value = AppConstants.DefaultSeed;
        }

        private void OnNewGameClicked()
        {
            _signalBus.Fire(_newGameSignalFactory.Create(new GameParameters(_seed.Value)));
        }

        private void OnCustomCampaignClicked()
        {
            // todo: start campaign
        }

        private void OnPlaygroundClicked()
        {
            // todo: start playground
        }
    }
}