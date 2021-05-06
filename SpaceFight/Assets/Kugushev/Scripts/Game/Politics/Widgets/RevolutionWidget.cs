using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Politics.Widgets
{
    public class RevolutionWidget : MonoBehaviour
    {
        [SerializeField] private Button? declareRevolutionButton;

        private Parliament? _model;

        public void SetUp(Parliament model)
        {
            _model = model;
        }

        public void UpdateView()
        {
            Asserting.NotNull(_model, declareRevolutionButton);

            int loyalPolitics = 0;
            foreach (var politician in _model.Politicians)
                if (politician.Relation == Relation.Loyalist)
                    loyalPolitics++;

            declareRevolutionButton.interactable = loyalPolitics >= GameConstants.LoyalPoliticsToWin;
        }
    }
}