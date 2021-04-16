using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Events;
using Kugushev.Scripts.Game.Models;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticianCardWidget : MonoBehaviour
    {
        [SerializeField] private PoliticianSelectedEvent onSelected;

        [SerializeField] private TextMeshProUGUI budgetValueLabel;

        [Header("Character")] [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI perkNameLabel;

        [Header("Relation")] [SerializeField] private GameObject relationEnemy;
        [SerializeField] private GameObject relationHater;
        [SerializeField] private GameObject relationIndifferent;
        [SerializeField] private GameObject relationPartner;
        [SerializeField] private GameObject relationLoyalist;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI traitBusinessValue;
        [SerializeField] private TextMeshProUGUI traitGreedValue;
        [SerializeField] private TextMeshProUGUI traitLustValue;
        [SerializeField] private TextMeshProUGUI traitBruteValue;
        [SerializeField] private TextMeshProUGUI traitVanityValue;

        const string UnknownTrait = "?";

        private Politician _model;

        public void SetUp(Politician model)
        {
            _model = model;
            UpdateView();
        }

        public void ToggleChanged(bool isOn)
        {
            if (!IsModelValid())
                return;

            onSelected?.Invoke(isOn ? _model : null);
        }

        public void UpdateView()
        {
            if (!IsModelValid())
                return;

            nameLabel.text = _model.Character.FullName;
            budgetValueLabel.text = StringBag.FromInt(_model.Budget);
            perkNameLabel.text = _model.Character.PerkLvl1.Caption;

            UpdateRelationView();
            UpdateTraitsView();
        }

        private void UpdateRelationView()
        {
            relationEnemy.SetActive(false);
            relationHater.SetActive(false);
            relationIndifferent.SetActive(false);
            relationPartner.SetActive(false);
            relationLoyalist.SetActive(false);

            switch (_model.Relation)
            {
                case Relation.Enemy:
                    relationEnemy.SetActive(true);
                    break;
                case Relation.Hater:
                    relationHater.SetActive(true);
                    break;
                case Relation.Indifferent:
                    relationIndifferent.SetActive(true);
                    break;
                case Relation.Partner:
                    relationPartner.SetActive(true);
                    break;
                case Relation.Loyalist:
                    relationLoyalist.SetActive(true);
                    break;
                default:
                    Debug.LogError($"Unexpected relation {_model.Relation}");
                    break;
            }
        }

        private void UpdateTraitsView()
        {
            UpdateTraitView(traitBusinessValue, _model.TraitsStatus.Business, _model.Traits.Business);
            UpdateTraitView(traitGreedValue, _model.TraitsStatus.Greed, _model.Traits.Greed);
            UpdateTraitView(traitLustValue, _model.TraitsStatus.Lust, _model.Traits.Lust);
            UpdateTraitView(traitBruteValue, _model.TraitsStatus.Brute, _model.Traits.Brute);
            UpdateTraitView(traitVanityValue, _model.TraitsStatus.Vanity, _model.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, bool revealed, int value) =>
                label.text = revealed ? StringBag.FromInt(value) : UnknownTrait;
        }

        private bool IsModelValid()
        {
            if (_model?.Active != true)
            {
                Debug.LogError($"Model is not active: {_model}");
                return false;
            }

            return true;
        }
    }
}