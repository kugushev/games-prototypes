using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class IntriguesWidget : MonoBehaviour
    {
        [SerializeField] private Button? applyButton;
        [SerializeField] private Transform? politicalActionsPanel;
        [SerializeField] private ToggleGroup? toggleGroup;
        [SerializeField] private GameObject? politicalActionCardPrefab;
        [SerializeField] private UnityEvent? onActionApplied;

        private IntrigueCardWidget? _selectedIntrigue;
        private GameModel? _rootModel;
        private IPoliticianSelector? _politicianSelector;

        public void Init(GameDateStore gameDateStore)
        {
            
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

        public void UpdateView()
        {
            Asserting.NotNull(applyButton, _politicianSelector);

            applyButton.interactable = _selectedIntrigue is { } &&
                                       _politicianSelector.SelectedPolitician is { };
        }

        private void OnCardSelected(IntrigueCardWidget? politicalAction)
        {
            _selectedIntrigue = politicalAction;
            UpdateView();
        }

        public void OnApplyButton()
        {
            Asserting.NotNull(_politicianSelector, _rootModel, applyButton);

            if (_selectedIntrigue is { } && _politicianSelector.SelectedPolitician is { })
            {
                var selectedModel = _selectedIntrigue.Model;
                _politicianSelector.SelectedPolitician.ApplyPoliticalAction(selectedModel.Intrigue);

                _rootModel.RemovePoliticalAction(selectedModel);

                Destroy(_selectedIntrigue.gameObject);
                onActionApplied?.Invoke();

                _selectedIntrigue = null;
                applyButton.interactable = false;
            }
        }
    }
}