using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class MissionCardWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? seedText;
        [SerializeField] private Toggle? toggle;

        [Header("Background Colors")] [SerializeField]
        private Image? background;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        [Header("Reward")] [SerializeField] private TextMeshProUGUI? rewardCaption;
        [SerializeField] private TextMeshProUGUI? intelValue;

        [SerializeField] private TextMeshProUGUI? traitBusinessValue;
        [SerializeField] private TextMeshProUGUI? traitGreedValue;
        [SerializeField] private TextMeshProUGUI? traitLustValue;
        [SerializeField] private TextMeshProUGUI? traitBruteValue;
        [SerializeField] private TextMeshProUGUI? traitVanityValue;

        private MissionInfo _model;
        private MissionSelection? _rootModel;

        public void SetUp(MissionInfo model, MissionSelection rootModel, ToggleGroup toggleGroup)
        {
            Asserting.NotNull(toggle);

            _model = model;
            _rootModel = rootModel;
            toggle.group = toggleGroup;
        }

        void Start()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            Asserting.NotNull(seedText, background);

            seedText.text = StringBag.FromInt(_model.Seed);
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

            UpdateRewardView(_model.Reward);
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
            Asserting.NotNull(_rootModel);

            if (selected)
                _rootModel.SelectedMission = _model;
            else if (_rootModel.SelectedMission == _model)
            {
                _rootModel.SelectedMission = null;
            }
        }
    }
}