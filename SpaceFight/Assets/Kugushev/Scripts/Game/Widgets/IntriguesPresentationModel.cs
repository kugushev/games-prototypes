using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ValueObjects;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Widgets
{
    public class IntriguesPresentationModel : MonoBehaviour
    {
        [SerializeField] private Button? applyButton;
        [SerializeField] private Transform? politicalActionsPanel;
        [SerializeField] private ToggleGroup toggleGroup = default!;
        [SerializeField] private GameObject? politicalActionCardPrefab;
        [SerializeField] private UnityEvent? onActionApplied;

        [Inject] private IntrigueCardPresentationModel.Factory _cardsFactory = default!;

        [Inject]
        public void Init(GameDateStore gameDateStore)
        {
            var model = gameDateStore.Intrigues;
            model.ObserveAdd().Subscribe(e => AddIntrigueCard(e.Value));
        }

        private void AddIntrigueCard(IntrigueRecord intrigue)
        {
           var card = _cardsFactory.Create(intrigue, toggleGroup);
           // todo: test it!!!
        }


        // public void Setup(GameModel rootModel, IPoliticianSelector politicianSelector)
        // {
        //     Asserting.NotNull(politicalActionCardPrefab, politicalActionsPanel, toggleGroup);
        //
        //     _rootModel = rootModel;
        //     _politicianSelector = politicianSelector;
        //
        //     // todo: use pool
        //     foreach (var model in _rootModel.PoliticalActions)
        //     {
        //         var go = Instantiate(politicalActionCardPrefab, politicalActionsPanel);
        //         var widget = go.GetComponent<IntrigueCardWidget>();
        //         widget.SetUp(model, toggleGroup, OnCardSelected);
        //     }
        // }

        // public void UpdateView()
        // {
        //     Asserting.NotNull(applyButton, _politicianSelector);
        //
        //     applyButton.interactable = _selectedIntrigue is { } &&
        //                                _politicianSelector.SelectedPolitician is { };
        // }
        //
        // // todo: this method should be executed from 2 signals (from Intrigue Card and from Politician Card
        // private void OnCardSelected(IntrigueCardPresentationModel? politicalAction)
        // {
        //     _selectedIntrigue = politicalAction;
        //     UpdateView();
        // }
        //
        // public void OnApplyButton()
        // {
        //     Asserting.NotNull(_politicianSelector, _rootModel, applyButton);
        //
        //     if (_selectedIntrigue is { } && _politicianSelector.SelectedPolitician is { })
        //     {
        //         var selectedModel = _selectedIntrigue.Model;
        //         _politicianSelector.SelectedPolitician.ApplyPoliticalAction(selectedModel.Intrigue);
        //
        //         _rootModel.RemovePoliticalAction(selectedModel);
        //
        //         Destroy(_selectedIntrigue.gameObject);
        //         onActionApplied?.Invoke();
        //
        //         _selectedIntrigue = null;
        //         applyButton.interactable = false;
        //     }
        // }
    }
}