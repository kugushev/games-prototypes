using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.Events;

namespace Kugushev.Scripts.Game.Widgets
{
    public class ParliamentWidget : MonoBehaviour
    {
        [SerializeField] private PoliticianCardWidget[]? politicianCards;
        [SerializeField] private PoliticianDetailsWidget? politicianDetailsWidget;
        [SerializeField] private UnityEvent? onPoliticianSelected;

        private Parliament? _model;

        public void Setup(Parliament model)
        {
            Asserting.NotNull(politicianCards);

            _model = model;

            if (politicianCards.Length != model.Politicians.Count)
            {
                Debug.LogError($"Inconsistent politicians count {politicianCards.Length != model.Politicians.Count}");
                return;
            }

            for (int i = 0; i < politicianCards.Length; i++)
                politicianCards[i].SetUp(model.Politicians[i]);
        }

        public void PoliticianSelected(Politician? politician)
        {
            Asserting.NotNull(_model, politicianDetailsWidget);

            _model.SelectedPolitician = politician;

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