using System.Collections.Generic;
using JetBrains.Annotations;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticalActionsWidget : MonoBehaviour
    {
        [SerializeField] private Button applyButton;
        [SerializeField] private Transform politicalActionsPanel;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject politicalActionCardPrefab;

        private PoliticalAction _selectedPoliticalAction;

        public void Setup(IReadOnlyList<PoliticalAction> models)
        {
            foreach (var model in models)
            {
                var go = Instantiate(politicalActionCardPrefab, politicalActionsPanel);
                var widget = go.GetComponent<PoliticalActionWidget>();
                widget.SetUp(model, toggleGroup, OnCardSelected);
            }
        }

        private void OnCardSelected([CanBeNull] PoliticalAction politicalAction)
        {
            _selectedPoliticalAction = politicalAction;
            applyButton.interactable = _selectedPoliticalAction != null;
        }
    }
}