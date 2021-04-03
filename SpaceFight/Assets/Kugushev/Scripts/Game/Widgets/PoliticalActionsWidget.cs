using System.Collections.Generic;
using JetBrains.Annotations;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticalActionsWidget : MonoBehaviour
    {
        [SerializeField] private Button applyButton;
        [SerializeField] private Transform politicalActionsPanel;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject politicalActionCardPrefab;
        [SerializeField] private UnityEvent onActionApplied;

        [CanBeNull] private PoliticalActionWidget _selectedPoliticalAction;
        private IPoliticianSelector _politicianSelector;

        public void Setup(IReadOnlyList<PoliticalAction> models, IPoliticianSelector politicianSelector)
        {
            _politicianSelector = politicianSelector;

            foreach (var model in models)
            {
                var go = Instantiate(politicalActionCardPrefab, politicalActionsPanel);
                var widget = go.GetComponent<PoliticalActionWidget>();
                widget.SetUp(model, toggleGroup, OnCardSelected);
            }
        }

        public void UpdateView()
        {
            applyButton.interactable = _selectedPoliticalAction != null &&
                                       _politicianSelector.SelectedPolitician != null;
        }

        private void OnCardSelected([CanBeNull] PoliticalActionWidget politicalAction)
        {
            _selectedPoliticalAction = politicalAction;
            UpdateView();
        }

        public void OnApplyButton()
        {
            if (_selectedPoliticalAction != null && _politicianSelector.SelectedPolitician != null)
            {
                _politicianSelector.SelectedPolitician.ApplyPoliticalAction(_selectedPoliticalAction.Model);

                Destroy(_selectedPoliticalAction.gameObject);
                onActionApplied?.Invoke();
            }
        }
    }
}