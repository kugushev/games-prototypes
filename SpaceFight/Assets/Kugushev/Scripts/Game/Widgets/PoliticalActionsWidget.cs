using System.Collections.Generic;
using JetBrains.Annotations;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticalActionsWidget : MonoBehaviour
    {
        [SerializeField] private PoliticsWidget politicsWidget;
        [SerializeField] private Button applyButton;
        [SerializeField] private Transform politicalActionsPanel;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject politicalActionCardPrefab;

        [CanBeNull] private PoliticalActionWidget _selectedPoliticalAction;
        private Parliament _parliamentModel;

        public void Setup(IReadOnlyList<PoliticalAction> models, Parliament parliamentModel)
        {
            _parliamentModel = parliamentModel;

            foreach (var model in models)
            {
                var go = Instantiate(politicalActionCardPrefab, politicalActionsPanel);
                var widget = go.GetComponent<PoliticalActionWidget>();
                widget.SetUp(model, toggleGroup, OnCardSelected);
            }
        }

        private void OnCardSelected([CanBeNull] PoliticalActionWidget politicalAction)
        {
            _selectedPoliticalAction = politicalAction;
            applyButton.interactable = _selectedPoliticalAction != null;
        }

        public void OnApplyButton()
        {
            if (_selectedPoliticalAction != null && _parliamentModel.SelectedPolitician != null)
            {
                _parliamentModel.SelectedPolitician.ApplyPoliticalAction(_selectedPoliticalAction.Model);
                
                Destroy(_selectedPoliticalAction.gameObject);
                politicsWidget.UpdateView();
            }
        }
    }
}