using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticalActionWidget : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private TextMeshProUGUI caption;
        [SerializeField] private TextMeshProUGUI intelValue;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI traitBusinessValue;
        [SerializeField] private TextMeshProUGUI traitGreedValue;
        [SerializeField] private TextMeshProUGUI traitLustValue;
        [SerializeField] private TextMeshProUGUI traitBruteValue;
        [SerializeField] private TextMeshProUGUI traitVanityValue;

        [Header("Background")] [SerializeField]
        private Image background;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        private PoliticalAction _model;
        private Action<PoliticalAction> _onCardSelected;

        public void ToggleChanged(bool isOn)
        {
            if (!IsModelValid())
                return;

            _onCardSelected?.Invoke(isOn ? _model : null);
        }

        public void SetUp(PoliticalAction model, ToggleGroup toggleGroup, Action<PoliticalAction> onCardSelected)
        {
            _model = model;
            toggle.group = toggleGroup;
            _onCardSelected = onCardSelected;
            UpdateView();
        }

        private void UpdateView()
        {
            if (!IsModelValid())
                return;

            caption.text = _model.Caption;
            intelValue.text = StringBag.FromInt(_model.Intel);
            UpdateDifficultyView();
            UpdateTraitsView();
        }

        private void UpdateDifficultyView()
        {
            switch (_model.Difficulty)
            {
                case Difficulty.Normal:
                    background.color = normal;
                    break;
                case Difficulty.Hard:
                    background.color = hard;
                    break;
                case Difficulty.Insane:
                    background.color = insane;
                    break;
                default:
                    Debug.LogError($"Unexpected difficulty {_model.Difficulty}");
                    break;
            }
        }

        private void UpdateTraitsView()
        {
            UpdateTraitView(traitBusinessValue, _model.Traits.Business);
            UpdateTraitView(traitGreedValue, _model.Traits.Greed);
            UpdateTraitView(traitLustValue, _model.Traits.Lust);
            UpdateTraitView(traitBruteValue, _model.Traits.Brute);
            UpdateTraitView(traitVanityValue, _model.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, int value) => label.text = StringBag.FromInt(value);
        }

        private bool IsModelValid()
        {
            if (_model == null)
            {
                Debug.LogError($"Model is not active: {_model}");
                return false;
            }

            return true;
        }
    }
}