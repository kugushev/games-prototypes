using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Widgets
{
    public class IntrigueCardPresentationModel : MonoBehaviour, IPoolable<IntrigueRecord, ToggleGroup, IMemoryPool>
    {
        [SerializeField] private Toggle toggle = default!;
        [SerializeField] private TextMeshProUGUI caption = default!;
        [SerializeField] private TextMeshProUGUI intelValue = default!;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI traitBusinessValue = default!;
        [SerializeField] private TextMeshProUGUI traitGreedValue = default!;
        [SerializeField] private TextMeshProUGUI traitLustValue = default!;
        [SerializeField] private TextMeshProUGUI traitBruteValue = default!;
        [SerializeField] private TextMeshProUGUI traitVanityValue = default!;

        [Header("Background")] [SerializeField]
        private Image? background;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        private IntrigueRecord? _model;

        void IPoolable<IntrigueRecord, ToggleGroup, IMemoryPool>.OnSpawned(IntrigueRecord p1, ToggleGroup p2,
            IMemoryPool p3)
        {
            _model = p1;
            toggle.group = p2;
        }

        void IPoolable<IntrigueRecord, ToggleGroup, IMemoryPool>.OnDespawned()
        {
            _model = default;
            toggle.group = default!;
        }

        public IntrigueRecord Model => _model ?? throw new SpaceFightException($"Model is null");

        #region UnityEngine

        private void Awake()
        {
            // toggle.OnValueChangedAsObservable().Where(s => _model != null).Subscribe(selected => fire)
        }

        #endregion

        public void ToggleChanged(bool isOn)
        {
            if (!IsModelValid())
                return;

            // todo: Fire signal
            //_onCardSelected?.Invoke(isOn ? this : null);
        }

        private void UpdateView()
        {
            Asserting.NotNull(caption, intelValue);

            if (!IsModelValid())
                return;

            caption.text = Model.Intrigue.Caption;
            intelValue.text = StringBag.FromInt(Model.Intrigue.Intel);
            UpdateDifficultyView();
            UpdateTraitsView();
        }

        private void UpdateDifficultyView()
        {
            Asserting.NotNull(background);

            switch (Model.Intrigue.Difficulty)
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
                    Debug.LogError($"Unexpected difficulty {Model.Intrigue.Difficulty}");
                    break;
            }
        }

        private void UpdateTraitsView()
        {
            Asserting.NotNull(traitBusinessValue, traitGreedValue, traitLustValue, traitBruteValue, traitVanityValue);

            UpdateTraitView(traitBusinessValue, Model.Intrigue.Traits.Business);
            UpdateTraitView(traitGreedValue, Model.Intrigue.Traits.Greed);
            UpdateTraitView(traitLustValue, Model.Intrigue.Traits.Lust);
            UpdateTraitView(traitBruteValue, Model.Intrigue.Traits.Brute);
            UpdateTraitView(traitVanityValue, Model.Intrigue.Traits.Vanity);

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

        public class Factory : PlaceholderFactory<IntrigueRecord, ToggleGroup, IntrigueCardPresentationModel>
        {
        }
    }
}