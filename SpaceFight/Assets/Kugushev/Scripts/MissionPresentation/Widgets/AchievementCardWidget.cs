using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Mission.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class AchievementCardWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI captionText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Toggle selector;

        private DebriefingSummary _rootModel;
        private AchievementInfo _model;

        public void SetUp(AchievementInfo model, DebriefingSummary rootModel, ToggleGroup toggleGroup)
        {
            _model = model;
            _rootModel = rootModel;
            selector.group = toggleGroup;
        }

        public void SelectionChanged(bool selected)
        {
            if (selected)
                _rootModel.SelectedAchievement = _model;
            else if (_rootModel.SelectedAchievement == _model)
                _rootModel.SelectedAchievement = null;
        }

        private void Start()
        {
            captionText.text = _model.Caption;
            descriptionText.text = _model.Description;
        }
    }
}