using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Game.Politics.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class IntriguesPresentationModel : MonoBehaviour, IIntriguesSelector, IIntriguesPresentationModel
    {
        [SerializeField] private ToggleGroup toggleGroup = default!;

        [Inject] private IIntrigues _model = default!;
        [Inject] private IntrigueCardPresentationModel.Factory _cardsFactory = default!;

        private readonly Dictionary<IntrigueCard, IntrigueCardPresentationModel> _intrigueCards =
            new Dictionary<IntrigueCard, IntrigueCardPresentationModel>(16);

        private readonly ReactiveProperty<IntrigueCard?> _selectedIntrigueCard = new ReactiveProperty<IntrigueCard?>();


        private void Start()
        {
            foreach (var card in _model.IntrigueCards)
                AddIntrigueCard(card);

            _model.IntrigueCards.ObserveAdd().Subscribe(e => AddIntrigueCard(e.Value));
            _model.IntrigueCards.ObserveRemove().Subscribe(e => RemoveIntrigueCard(e.Value));
        }

        IReadOnlyReactiveProperty<IntrigueCard?> IIntriguesSelector.SelectedIntrigue => _selectedIntrigueCard;

        ToggleGroup IIntriguesPresentationModel.ToggleGroup => toggleGroup;
        void IIntriguesPresentationModel.SelectCard(IntrigueCard? card) => _selectedIntrigueCard.Value = card;

        private void AddIntrigueCard(IntrigueCard intrigue)
        {
            if (_intrigueCards.ContainsKey(intrigue))
            {
                Debug.LogError($"Intrigue {intrigue} is already set");
                return;
            }

            var card = _cardsFactory.Create(intrigue, this);
            _intrigueCards.Add(intrigue, card);
        }

        private void RemoveIntrigueCard(IntrigueCard intrigue)
        {
            if (_selectedIntrigueCard.Value == intrigue)
                _selectedIntrigueCard.Value = null;

            if (_intrigueCards.TryGetValue(intrigue, out var card))
            {
                card.Dispose();
                _intrigueCards.Remove(intrigue);
            }
            else
                Debug.LogError($"Intrigue {intrigue} is not set");
        }
    }
}