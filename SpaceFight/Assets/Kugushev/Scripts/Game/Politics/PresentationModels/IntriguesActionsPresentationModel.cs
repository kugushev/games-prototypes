using Kugushev.Scripts.Game.Core.Models;
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

        private static bool BothSelected(IPolitician? politician, IntrigueCard? intrigue) =>
            politician is { } && intrigue is { };

        private void ApplyIntrigueToPolitician(IPolitician politician, IntrigueCard intrigue)
        {
            // todo: signal to model
        }
    }
}