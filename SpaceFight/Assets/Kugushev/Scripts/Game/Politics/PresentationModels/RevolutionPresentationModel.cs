using System.Linq;
using Kugushev.Scripts.Game.Core;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class RevolutionPresentationModel : MonoBehaviour
    {
        [SerializeField] private Button declareRevolutionButton = default!;

        private Parliament? _model;

        [Inject]
        private void Init(GameDataStore dataStore)
        {
            _model = dataStore.Parliament;

            foreach (var politician in _model.Politicians)
                politician.Relation.Subscribe(_ => UpdateView());
        }

        private void UpdateView()
        {
            if (_model is null)
                return;

            var loyalPolitics = _model.Politicians.Count(p => p.Relation.Value == Relation.Loyalist);

            declareRevolutionButton.interactable = loyalPolitics >= GameConstants.LoyalPoliticsToWin;
        }
    }
}