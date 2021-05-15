using System;
using Kugushev.Scripts.Campaign.Core.ValueObjects;
using Kugushev.Scripts.Campaign.MissionSelection.Interfaces;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Campaign.MissionSelection.PresentationModels
{
    public class MissionCardPresentationModel : MonoBehaviour,
        IPoolable<MissionInfo, IMissionSelectionPresentationModel, IMemoryPool>,
        IDisposable
    {
        [SerializeField] private TextMeshProUGUI seedText = default!;
        [SerializeField] private Toggle toggle = default!;

        [Header("Background Colors")] [SerializeField]
        private Image background = default!;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        [Header("Reward")] [SerializeField] private TextMeshProUGUI rewardCaption = default!;
        [SerializeField] private TextMeshProUGUI intelValue = default!;

        [SerializeField] private TextMeshProUGUI traitBusinessValue = default!;
        [SerializeField] private TextMeshProUGUI traitGreedValue = default!;
        [SerializeField] private TextMeshProUGUI traitLustValue = default!;
        [SerializeField] private TextMeshProUGUI traitBruteValue = default!;
        [SerializeField] private TextMeshProUGUI traitVanityValue = default!;

        private MissionInfo? _model;
        private IMissionSelectionPresentationModel? _parent;
        private IMemoryPool? _pool;

        #region IPoolable, IDisposable

        void IPoolable<MissionInfo, IMissionSelectionPresentationModel, IMemoryPool>.OnSpawned(MissionInfo p1,
            IMissionSelectionPresentationModel p2, IMemoryPool p3)
        {
            _model = p1;
            _parent = p2;
            _pool = p3;

            toggle.group = p2.ToggleGroup;

            UpdateView(_model);
        }

        void IPoolable<MissionInfo, IMissionSelectionPresentationModel, IMemoryPool>.OnDespawned()
        {
            _model = default;
            _parent = default;
            _pool = default;

            toggle.group = default!;
        }

        public void Dispose() => _pool?.Despawn(this);

        #endregion

        private void UpdateView(MissionInfo model)
        {
            Asserting.NotNull(seedText, background);

            seedText.text = StringBag.FromInt(model.Seed);
            switch (model.Difficulty)
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
                    Debug.LogError($"Unexpected difficulty {model.Difficulty}");
                    break;
            }

            UpdateRewardView(model.Reward);
        }

        private void UpdateRewardView(Intrigue reward)
        {
            Asserting.NotNull(rewardCaption, intelValue, traitBusinessValue, traitGreedValue, traitLustValue,
                traitBruteValue, traitVanityValue);

            rewardCaption.text = reward.Caption;
            intelValue.text = StringBag.FromInt(reward.Intel);

            UpdateTraitView(traitBusinessValue, reward.Traits.Business);
            UpdateTraitView(traitGreedValue, reward.Traits.Greed);
            UpdateTraitView(traitLustValue, reward.Traits.Lust);
            UpdateTraitView(traitBruteValue, reward.Traits.Brute);
            UpdateTraitView(traitVanityValue, reward.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, int value) => label.text = StringBag.FromInt(value);
        }

        public void OnToggleChanged(bool selected)
        {
            if (_parent is null)
                return;

            if (selected && _model is { })
                _parent.SelectCard(_model);

            if (!selected)
                _parent.SelectCard(null);
        }

        public class Factory : PlaceholderFactory<MissionInfo, IMissionSelectionPresentationModel,
            MissionCardPresentationModel>
        {
        }
    }
}