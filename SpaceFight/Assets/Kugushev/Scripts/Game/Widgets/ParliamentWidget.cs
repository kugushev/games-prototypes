using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Kugushev.Scripts.Game.Widgets
{
    public class ParliamentWidget : MonoBehaviour
    {
        [SerializeField] private PoliticianCardWidget[] politicianCards = default!;
        [SerializeField] private PoliticianDetailsWidget politicianDetailsWidget = default!;
        [SerializeField] private UnityEvent onPoliticianSelected = default!;

        private Parliament _model = default!;

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

        public IPolitician? SelectedPolitician { get; private set; }


        public void PoliticianSelected(IPolitician? politician)
        {
            SelectedPolitician = politician;

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