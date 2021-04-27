using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class IntrigueCardWidget : MonoBehaviour
    {
        [SerializeField] private Toggle? toggle;
        [SerializeField] private TextMeshProUGUI? caption;
        [SerializeField] private TextMeshProUGUI? intelValue;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI? traitBusinessValue;
        [SerializeField] private TextMeshProUGUI? traitGreedValue;
        [SerializeField] private TextMeshProUGUI? traitLustValue;
        [SerializeField] private TextMeshProUGUI? traitBruteValue;
        [SerializeField] private TextMeshProUGUI? traitVanityValue;

        [Header("Background")] [SerializeField]
        private Image? background;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        private IntrigueRecord _model;
        private Action<IntrigueCardWidget?>? _onCardSelected;

        public IntrigueRecord Model => _model;

        public void ToggleChanged(bool isOn)
        {
            if (!IsModelValid())
                return;

            _onCardSelected?.Invoke(isOn ? this : null);
        }

        public void SetUp(IntrigueRecord model, ToggleGroup toggleGroup,
            Action<IntrigueCardWidget?> onCardSelected)
        {
            Asserting.NotNull(toggle, onCardSelected);

            _model = model;
            toggle.group = toggleGroup;
            _onCardSelected = onCardSelected;
            UpdateView();
        }

        private void UpdateView()
        {
            Asserting.NotNull(caption, intelValue);

            if (!IsModelValid())
                return;

            caption.text = _model.Intrigue.Caption;
            intelValue.text = StringBag.FromInt(_model.Intrigue.Intel);
            UpdateDifficultyView();
            UpdateTraitsView();
        }

        private void UpdateDifficultyView()
        {
            Asserting.NotNull(background);

            switch (_model.Intrigue.Difficulty)
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
                    Debug.LogError($"Unexpected difficulty {_model.Intrigue.Difficulty}");
                    break;
            }
        }

        private void UpdateTraitsView()
        {
            Asserting.NotNull(traitBusinessValue, traitGreedValue, traitLustValue, traitBruteValue, traitVanityValue);
            
            UpdateTraitView(traitBusinessValue, _model.Intrigue.Traits.Business);
            UpdateTraitView(traitGreedValue, _model.Intrigue.Traits.Greed);
            UpdateTraitView(traitLustValue, _model.Intrigue.Traits.Lust);
            UpdateTraitView(traitBruteValue, _model.Intrigue.Traits.Brute);
            UpdateTraitView(traitVanityValue, _model.Intrigue.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, int value) => label.text = StringBag.FromInt(value);
        }

        private bool IsModelValid()
        {
            if (_model == default)
            {
                Debug.LogError($"Model is not active: {_model}");
                return false;
            }

            return true;
        }
    }
}