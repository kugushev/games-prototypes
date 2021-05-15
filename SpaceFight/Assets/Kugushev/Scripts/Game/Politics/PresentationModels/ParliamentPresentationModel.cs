using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Politics.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class ParliamentPresentationModel : MonoBehaviour, IParliamentPresentationModel, IPoliticianSelector
    {
        [SerializeField] private PoliticianCardPresentationModel[] politicianCards = default!;

        private Parliament _model = default!;
        private readonly ReactiveProperty<IPolitician?> _selectedPolitician = new ReactiveProperty<IPolitician?>();

        [Inject]
        public void Init(Parliament parliament)
        {
            _model = parliament;

            if (politicianCards.Length != _model.Politicians.Count)
            {
                Debug.LogError($"Inconsistent politicians count {politicianCards.Length != _model.Politicians.Count}");
                return;
            }

            for (int i = 0; i < politicianCards.Length; i++)
                politicianCards[i].SetUp(_model.Politicians[i], this);
        }

        public IReadOnlyReactiveProperty<IPolitician?> SelectedPolitician => _selectedPolitician;

        public void SelectPolitician(IPolitician? politician)
        {
            _selectedPolitician.Value = politician;
        }
    }
}