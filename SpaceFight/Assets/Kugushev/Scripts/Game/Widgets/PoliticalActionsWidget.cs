using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticalActionsWidget : MonoBehaviour
    {
        [SerializeField] private Button? applyButton;
        [SerializeField] private Transform? politicalActionsPanel;
        [SerializeField] private ToggleGroup? toggleGroup;
        [SerializeField] private GameObject? politicalActionCardPrefab;
        [SerializeField] private UnityEvent? onActionApplied;

        private PoliticalActionWidget? _selectedPoliticalAction;
        private GameModel? _rootModel;
        private IPoliticianSelector? _politicianSelector;


        public void Setup(GameModel rootModel, IPoliticianSelector politicianSelector)
        {
            Asserting.NotNull(politicalActionCardPrefab, politicalActionsPanel, toggleGroup);

            _rootModel = rootModel;
            _politicianSelector = politicianSelector;

            // todo: use pool
            foreach (var model in _rootModel.PoliticalActions)
            {
                var go = Instantiate(politicalActionCardPrefab, politicalActionsPanel);
                var widget = go.GetComponent<PoliticalActionWidget>();
                widget.SetUp(model, toggleGroup, OnCardSelected);
            }
        }

        public void UpdateView()
        {
            Asserting.NotNull(applyButton, _politicianSelector);

            applyButton.interactable = _selectedPoliticalAction is { } &&
                                       _politicianSelector.SelectedPolitician is { };
        }

        private void OnCardSelected(PoliticalActionWidget? politicalAction)
        {
            _selectedPoliticalAction = politicalAction;
            UpdateView();
        }

        public void OnApplyButton()
        {
            Asserting.NotNull(_politicianSelector, _rootModel, applyButton);

            if (_selectedPoliticalAction is { } && _politicianSelector.SelectedPolitician is { })
            {
                var selectedModel = _selectedPoliticalAction.Model;
                _politicianSelector.SelectedPolitician.ApplyPoliticalAction(selectedModel.PoliticalAction);

                _rootModel.RemovePoliticalAction(selectedModel);

                Destroy(_selectedPoliticalAction.gameObject);
                onActionApplied?.Invoke();

                _selectedPoliticalAction = null;
                applyButton.interactable = false;
            }
        }
    }
}