using JetBrains.Annotations;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
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
        private GameModel _rootModel;
        private IPoliticianSelector _politicianSelector;


        public void Setup(GameModel rootModel, IPoliticianSelector politicianSelector)
        {
            _rootModel = rootModel;
            _politicianSelector = politicianSelector;

            // todo: use prefab pool
            foreach (var model in _rootModel.PoliticalActions)
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