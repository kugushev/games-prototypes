using JetBrains.Annotations;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Widgets
{
    public class ParliamentWidget : MonoBehaviour
    {
        [SerializeField] private PoliticianCardWidget[] politicianCards;
        [SerializeField] private PoliticianDetailsWidget politicianDetailsWidget;

        private Parliament _model;

        public void Setup(Parliament model)
        {
            _model = model;

            if (politicianCards.Length != model.Politicians.Count)
            {
                Debug.LogError($"Inconsistent politicians count {politicianCards.Length != model.Politicians.Count}");
                return;
            }

            for (int i = 0; i < politicianCards.Length; i++)
                politicianCards[i].SetUp(model.Politicians[i]);
        }

        public void PoliticianSelected([CanBeNull] Politician politician)
        {
            if (politician == null)
            {
                politicianDetailsWidget.Deselect();
                return;
            }

            politicianDetailsWidget.Select(politician);
        }
    }
}