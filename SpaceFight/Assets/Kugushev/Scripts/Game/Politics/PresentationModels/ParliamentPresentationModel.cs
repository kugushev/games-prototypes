using Kugushev.Scripts.Game.Core;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Politics.Interfaces;
using Kugushev.Scripts.Game.Politics.Widgets;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class ParliamentPresentationModel : MonoBehaviour, IPoliticianSelector
    {
        [SerializeField] private PoliticianCardWidget[] politicianCards = default!;

        private Parliament _model = default!;
        private readonly ReactiveProperty<IPolitician?> _selectedPolitician = new ReactiveProperty<IPolitician?>();

        [Inject]
        public void Init(GameDateStore gameDateStore)
        {
            _model = gameDateStore.Parliament;

            if (politicianCards.Length != _model.Politicians.Count)
            {
                Debug.LogError($"Inconsistent politicians count {politicianCards.Length != _model.Politicians.Count}");
                return;
            }

            for (int i = 0; i < politicianCards.Length; i++)
                politicianCards[i].SetUp(_model.Politicians[i]);
        }

        public IReadOnlyReactiveProperty<IPolitician?> SelectedPolitician => _selectedPolitician;

        public void PoliticianSelected(IPolitician? politician)
        {
            _selectedPolitician.Value = politician;
        }
    }
}