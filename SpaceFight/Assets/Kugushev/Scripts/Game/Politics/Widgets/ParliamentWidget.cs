using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Kugushev.Scripts.Game.Widgets
{
    public class ParliamentWidget : MonoBehaviour, IPoliticianSelector
    {
        [SerializeField] private PoliticianCardWidget[] politicianCards = default!;
        [SerializeField] private PoliticianDetailsWidget politicianDetailsWidget = default!;
        [SerializeField] private UnityEvent onPoliticianSelected = default!;

        private Parliament _model = default!;
        private ReactiveProperty<IPolitician?> _selectedPolitician;

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

            if (politician == null)
                politicianDetailsWidget.Deselect();
            else
                politicianDetailsWidget.Select(politician);

            onPoliticianSelected?.Invoke();
        }

        public void UpdateView()
        {
            Asserting.NotNull(politicianCards, politicianDetailsWidget);

            foreach (var politicianCard in politicianCards)
                politicianCard.UpdateView();
            politicianDetailsWidget.UpdateView();
        }
    }
}