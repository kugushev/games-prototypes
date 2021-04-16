using System.Diagnostics.CodeAnalysis;
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
        [SerializeField] private PoliticianSelectedEvent? onSelected;

        [SerializeField] private TextMeshProUGUI? budgetValueLabel;

        [Header("Character")] [SerializeField] private TextMeshProUGUI? nameLabel;
        [SerializeField] private TextMeshProUGUI? perkNameLabel;

        [Header("Relation")] [SerializeField] private GameObject? relationEnemy;
        [SerializeField] private GameObject? relationHater;
        [SerializeField] private GameObject? relationIndifferent;
        [SerializeField] private GameObject? relationPartner;
        [SerializeField] private GameObject? relationLoyalist;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI? traitBusinessValue;
        [SerializeField] private TextMeshProUGUI? traitGreedValue;
        [SerializeField] private TextMeshProUGUI? traitLustValue;
        [SerializeField] private TextMeshProUGUI? traitBruteValue;
        [SerializeField] private TextMeshProUGUI? traitVanityValue;

        const string UnknownTrait = "?";

        private Politician? _model;

        public void SetUp(Politician model)
        {
            _model = model;
            UpdateView();
        }

        public void ToggleChanged(bool isOn)
        {
            if (!IsModelValid(out var model))
                return;

            onSelected?.Invoke(isOn ? model : null);
        }

        public void UpdateView()
        {
            Asserting.NotNull(nameLabel, budgetValueLabel, perkNameLabel);

            if (!IsModelValid(out var model))
                return;

            nameLabel.text = model.Character.FullName;
            budgetValueLabel.text = StringBag.FromInt(model.Budget);
            perkNameLabel.text = model.Character.PerkLvl1.Caption;

            UpdateRelationView(model);
            UpdateTraitsView(model);
        }

        private void UpdateRelationView(Politician model)
        {
            Asserting.NotNull(relationEnemy, relationHater, relationIndifferent, relationPartner, relationLoyalist);

            relationEnemy.SetActive(false);
            relationHater.SetActive(false);
            relationIndifferent.SetActive(false);
            relationPartner.SetActive(false);
            relationLoyalist.SetActive(false);

            switch (model.Relation)
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
                    Debug.LogError($"Unexpected relation {model.Relation}");
                    break;
            }
        }

        private void UpdateTraitsView(Politician model)
        {
            Asserting.NotNull(traitBusinessValue, traitGreedValue, traitLustValue, traitBruteValue, traitVanityValue);

            UpdateTraitView(traitBusinessValue, model.TraitsStatus.Business, model.Traits.Business);
            UpdateTraitView(traitGreedValue, model.TraitsStatus.Greed, model.Traits.Greed);
            UpdateTraitView(traitLustValue, model.TraitsStatus.Lust, model.Traits.Lust);
            UpdateTraitView(traitBruteValue, model.TraitsStatus.Brute, model.Traits.Brute);
            UpdateTraitView(traitVanityValue, model.TraitsStatus.Vanity, model.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, bool revealed, int value) =>
                label.text = revealed ? StringBag.FromInt(value) : UnknownTrait;
        }

        private bool IsModelValid([NotNullWhen(true)] out Politician? model)
        {
            if (_model?.Active != true)
            {
                Debug.LogError($"Model is not active: {_model}");
                model = null;
                return false;
            }

            model = _model;
            return true;
        }
    }
}