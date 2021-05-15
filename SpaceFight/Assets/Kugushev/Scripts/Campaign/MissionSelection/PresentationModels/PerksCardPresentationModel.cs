using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.ValueObjects;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.MissionSelection.PresentationModels
{
    public class PerksCardPresentationModel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI captionText = default!;

        private PerkInfo _model;

        public void Init(PerkInfo model) => _model = model;

        private void Start()
        {
            Asserting.NotNull(captionText);
            
            captionText.text = _model.Caption;
        }
    }
}