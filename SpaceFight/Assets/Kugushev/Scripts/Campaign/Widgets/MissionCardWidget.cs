using System;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class MissionCardWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI seedText;
        [SerializeField] private TextMeshProUGUI rewardText;
        [SerializeField] private Toggle toggle;

        [Header("Background Colors")] [SerializeField]
        private Image background;

        [SerializeField] private Color normal;
        [SerializeField] private Color hard;
        [SerializeField] private Color insane;

        private MissionInfo _model;
        private MissionSelection _rootModel;

        public void SetUp(MissionInfo model, MissionSelection rootModel, ToggleGroup toggleGroup)
        {
            _model = model;
            _rootModel = rootModel;
            toggle.group = toggleGroup;
        }

        void Start()
        {
            seedText.text = StringBag.FromInt(_model.Seed);
            rewardText.text = "Lorem ipsum" + Environment.NewLine + "Lust: 3" + Environment.NewLine + "Greed: -2";

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

        public void OnToggleChanged(bool selected)
        {
            if (selected)
                _rootModel.SelectedMission = _model;
            else if (_rootModel.SelectedMission == _model)
            {
                _rootModel.SelectedMission = null;
            }
        }
    }
}