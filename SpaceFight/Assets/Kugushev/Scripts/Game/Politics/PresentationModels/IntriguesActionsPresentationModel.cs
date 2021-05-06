using System;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ValueObjects;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Widgets
{
    public class IntriguesActionsPresentationModel : MonoBehaviour
    {
        [Inject] private IPoliticianSelector _politicianSelector = default!;
        [Inject] private IIntriguesSelector _intriguesSelector = default!;

        [SerializeField] private Button applyIntrigueButton = default!;

        private void Awake()
        {
            _politicianSelector.SelectedPolitician
                .CombineLatest(_intriguesSelector.SelectedIntrigue, BothSelected)
                .SubscribeToInteractable(applyIntrigueButton);

            applyIntrigueButton.OnClickAsObservable()
                .CombineLatest(_politicianSelector.SelectedPolitician, (_, politician) => politician)
                .CombineLatest(_intriguesSelector.SelectedIntrigue, (politician, intrigue) => (politician, intrigue))
                .Where(pair => BothSelected(pair.politician, pair.intrigue))
                .Subscribe(pair => ApplyIntrigueToPolitician(pair.politician!, pair.intrigue!));
        }

        private static bool BothSelected(IPolitician? politician, IntrigueRecord? intrigue) =>
            politician is { } && intrigue is { };

        private void ApplyIntrigueToPolitician(IPolitician politician, IntrigueRecord intrigue)
        {
            // todo: signal
        }
    }
}