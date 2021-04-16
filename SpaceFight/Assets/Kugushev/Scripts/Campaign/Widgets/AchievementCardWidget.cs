using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.ValueObjects;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class AchievementCardWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? captionText;

        private PerkInfo _model;

        public void SetUp(PerkInfo model) => _model = model;

        private void Start()
        {
            Asserting.NotNull(captionText);
            
            captionText.text = _model.Caption;
        }
    }
}