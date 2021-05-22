using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Game.Politics.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class IntriguesActionsPresentationModel : MonoBehaviour
    {
        [SerializeField] private Button applyIntrigueButton = default!;

        [Inject] private IPoliticianSelector _politicianSelector = default!;
        [Inject] private IIntriguesSelector _intriguesSelector = default!;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private ApplyIntrigueCard.Factory _applyIntrigueCardFactory = default!;


        private void Awake()
        {
            _politicianSelector.SelectedPolitician
                .CombineLatest(_intriguesSelector.SelectedIntrigue, BothSelected)
                .SubscribeToInteractable(applyIntrigueButton)
                .AddTo(this);

            applyIntrigueButton.onClick.AddListener(OnApplyButtonClick);
        }

        private void OnApplyButtonClick()
        {
            var politician = _politicianSelector.SelectedPolitician.Value;
            var intrigue = _intriguesSelector.SelectedIntrigue.Value;

            if (BothSelected(politician, intrigue))
            {
                var signal = _applyIntrigueCardFactory.Create((intrigue, politician));
                _signalBus.Fire(signal);
            }
        }

        private static bool BothSelected(
            [NotNullWhen(true)] IPolitician? politician,
            [NotNullWhen(true)] IntrigueCard? intrigue) =>
            politician is { } && intrigue is { };
    }
}