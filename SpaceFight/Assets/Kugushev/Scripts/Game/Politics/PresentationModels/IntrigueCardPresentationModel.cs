using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Game.Politics.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class IntrigueCardPresentationModel : MonoBehaviour,
        IPoolable<IntrigueCard, IIntriguesPresentationModel, IMemoryPool>,
        IDisposable
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
        private Image background = default!;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        private IntrigueCard? _model;
        private IIntriguesPresentationModel? _parent;
        private IMemoryPool? _pool;

        #region IPoolable, IDisposable

        void IPoolable<IntrigueCard, IIntriguesPresentationModel, IMemoryPool>.OnSpawned(IntrigueCard p1,
            IIntriguesPresentationModel p2,
            IMemoryPool p3)
        {
            _model = p1;
            _parent = p2;
            _pool = p3;

            toggle.group = _parent.ToggleGroup;

            UpdateView(_model);
        }

        void IPoolable<IntrigueCard, IIntriguesPresentationModel, IMemoryPool>.OnDespawned()
        {
            _model = default;
            _parent = default;
            _pool = default;

            toggle.group = default!;
        }

        public void Dispose() => _pool?.Despawn(this);

        #endregion

        #region UnityEngine

        private void Awake()
        {
            toggle.OnValueChangedAsObservable()
                .Where(selected => selected)
                .Subscribe(_ => OnCardSelected());
        }

        #endregion

        private void OnCardSelected()
        {
            if (_model is { } && _parent is { })
                _parent!.SelectCard(_model!);
        }

        private void UpdateView(IntrigueCard model)
        {
            caption.text = model.Intrigue.Caption;
            intelValue.text = StringBag.FromInt(model.Intrigue.Intel);
            UpdateDifficultyView(model);
            UpdateTraitsView(model);
        }

        private void UpdateDifficultyView(IntrigueCard model)
        {
            switch (model.Intrigue.Difficulty)
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
                    Debug.LogError($"Unexpected difficulty {model.Intrigue.Difficulty}");
                    break;
            }
        }

        private void UpdateTraitsView(IntrigueCard model)
        {
            UpdateTraitView(traitBusinessValue, model.Intrigue.Traits.Business);
            UpdateTraitView(traitGreedValue, model.Intrigue.Traits.Greed);
            UpdateTraitView(traitLustValue, model.Intrigue.Traits.Lust);
            UpdateTraitView(traitBruteValue, model.Intrigue.Traits.Brute);
            UpdateTraitView(traitVanityValue, model.Intrigue.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, int value) => label.text = StringBag.FromInt(value);
        }

        public class Factory : PlaceholderFactory<IntrigueCard, IIntriguesPresentationModel,
            IntrigueCardPresentationModel>
        {
        }
    }
}