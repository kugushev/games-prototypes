using System.Linq;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
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

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private SignalToTransition<RevolutionParameters>.Factory _declareRevolutionFactory = default!;

        private Parliament _model = default!;

        [Inject]
        private void Init(Parliament parliament) => _model = parliament;

        private void Awake()
        {
            foreach (var politician in _model.Politicians)
                politician.Relation.Subscribe(_ => UpdateView()).AddTo(this);

            declareRevolutionButton.onClick.AddListener(OnDeclareRevolutionClick);
        }

        private void OnDeclareRevolutionClick() => _signalBus.Fire(_declareRevolutionFactory.Create(default));

        private void UpdateView()
        {
            var loyalPolitics = _model.Politicians.Count(p => p.Relation.Value == Relation.Loyalist);

            declareRevolutionButton.interactable = loyalPolitics >= GameConstants.LoyalPoliticsToWin;
        }
    }
}