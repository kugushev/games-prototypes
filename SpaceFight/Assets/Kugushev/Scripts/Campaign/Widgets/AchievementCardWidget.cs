using Kugushev.Scripts.App.ValueObjects;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class AchievementCardWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI captionText;

        private AchievementInfo _model;

        public void SetUp(AchievementInfo model) => _model = model;

        private void Start() => captionText.text = _model.Caption;
    }
}