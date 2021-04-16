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

        private PoliticalActionInfo _model;
        private Action<PoliticalActionWidget?>? _onCardSelected;

        public PoliticalActionInfo Model => _model;

        public void ToggleChanged(bool isOn)
        {
            if (!IsModelValid())
                return;

            _onCardSelected?.Invoke(isOn ? this : null);
        }

        public void SetUp(PoliticalActionInfo model, ToggleGroup toggleGroup,
            Action<PoliticalActionWidget?> onCardSelected)
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

            caption.text = _model.PoliticalAction.Caption;
            intelValue.text = StringBag.FromInt(_model.PoliticalAction.Intel);
            UpdateDifficultyView();
            UpdateTraitsView();
        }

        private void UpdateDifficultyView()
        {
            Asserting.NotNull(background);

            switch (_model.PoliticalAction.Difficulty)
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
                    Debug.LogError($"Unexpected difficulty {_model.PoliticalAction.Difficulty}");
                    break;
            }
        }

        private void UpdateTraitsView()
        {
            Asserting.NotNull(traitBusinessValue, traitGreedValue, traitLustValue, traitBruteValue, traitVanityValue);
            
            UpdateTraitView(traitBusinessValue, _model.PoliticalAction.Traits.Business);
            UpdateTraitView(traitGreedValue, _model.PoliticalAction.Traits.Greed);
            UpdateTraitView(traitLustValue, _model.PoliticalAction.Traits.Lust);
            UpdateTraitView(traitBruteValue, _model.PoliticalAction.Traits.Brute);
            UpdateTraitView(traitVanityValue, _model.PoliticalAction.Traits.Vanity);

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