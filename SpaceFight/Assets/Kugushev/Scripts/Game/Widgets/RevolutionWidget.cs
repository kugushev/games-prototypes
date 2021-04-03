using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class RevolutionWidget : MonoBehaviour
    {
        [SerializeField] private Button declareRevolutionButton;

        private Parliament _model;

        public void SetUp(Parliament model)
        {
            _model = model;
        }

        public void UpdateView()
        {
            int loyalPolitics = 0;
            foreach (var politician in _model.Politicians)
                if (politician.Relation == Relation.Loyalist)
                    loyalPolitics++;

            declareRevolutionButton.interactable = loyalPolitics >= GameConstants.LoyalPoliticsToWin;
        }
    }
}