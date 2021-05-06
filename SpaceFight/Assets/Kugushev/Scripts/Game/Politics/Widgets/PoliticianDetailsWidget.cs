using System;
using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticianDetailsWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? nameLabel;

        [Header("Relation")] [SerializeField] private TextMeshProUGUI? relationValueLabel;
        [SerializeField] private TextMeshProUGUI? relationTip;
        [SerializeField] private GameObject? relationEnemy;
        [SerializeField] private GameObject? relationHater;
        [SerializeField] private GameObject? relationIndifferent;
        [SerializeField] private GameObject? relationPartner;
        [SerializeField] private GameObject? relationLoyalist;

        [Header("Budget")] [SerializeField] private TextMeshProUGUI? budgetValueLabel;
        [SerializeField] private GameObject? budgetIsReadyToInvest;
        [SerializeField] private GameObject? budgetIsNotReadyToInvest;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI? traitBusinessValue;
        [SerializeField] private TextMeshProUGUI? traitGreedValue;
        [SerializeField] private TextMeshProUGUI? traitLustValue;
        [SerializeField] private TextMeshProUGUI? traitBruteValue;
        [SerializeField] private TextMeshProUGUI? traitVanityValue;

        [Header("Perks")] [SerializeField] private TextMeshProUGUI? perkNameLabel;
        [SerializeField] private TextMeshProUGUI? perkLvl1Requirement;
        [SerializeField] private TextMeshProUGUI? perkLvl1Effect;
        [SerializeField] private TextMeshProUGUI? perkLvl2Requirement;
        [SerializeField] private TextMeshProUGUI? perkLvl2Effect;
        [SerializeField] private TextMeshProUGUI? perkLvl3Requirement;
        [SerializeField] private TextMeshProUGUI? perkLvl3Effect;

        const string UnknownTrait = "?";

        private IPolitician? _model;

        public void Select(IPolitician model)
        {
            gameObject.SetActive(true);
            _model = model;
            UpdateView();
        }

        public void Deselect()
        {
            _model = null;
            gameObject.SetActive(false);
        }

        public void UpdateView()
        {
            Asserting.NotNull(nameLabel);

            if (!IsModelValid(out var model))
                return;

            nameLabel.text = model.Character.FullName;
            UpdateRelationView(model);
            UpdateBudgetView(model);
            UpdateTraitsView(model);
            UpdatePerksView(model);
        }

        private void UpdateRelationView(IPolitician model)
        {
            Asserting.NotNull(relationValueLabel, relationEnemy, relationHater, relationIndifferent, relationPartner,
                relationLoyalist, relationTip);

            relationValueLabel.text = StringBag.FromInt(model.RelationLevel);

            relationEnemy.SetActive(false);
            relationHater.SetActive(false);
            relationIndifferent.SetActive(false);
            relationPartner.SetActive(false);
            relationLoyalist.SetActive(false);

            switch (model.Relation)
            {
                case Relation.Enemy:
                    relationEnemy.SetActive(true);
                    relationTip.text = String.Empty;
                    break;
                case Relation.Hater:
                    relationHater.SetActive(true);
                    relationTip.text = String.Empty;
                    break;
                case Relation.Indifferent:
                    relationIndifferent.SetActive(true);
                    relationTip.text = String.Empty;
                    break;
                case Relation.Partner:
                    relationPartner.SetActive(true);
                    relationTip.text = "Will invest in your campaigns";
                    break;
                case Relation.Loyalist:
                    relationLoyalist.SetActive(true);
                    relationTip.text = "Will vote for you";
                    break;
                default:
                    Debug.LogError($"Unexpected relation {model.Relation}");
                    break;
            }
        }

        private void UpdateBudgetView(IPolitician model)
        {
            Asserting.NotNull(budgetValueLabel, budgetIsReadyToInvest, budgetIsNotReadyToInvest);

            budgetValueLabel.text = StringBag.FromInt(model.Budget);
            budgetIsReadyToInvest.SetActive(model.IsReadyToInvest);
            budgetIsNotReadyToInvest.SetActive(!model.IsReadyToInvest);
        }

        private void UpdateTraitsView(IPolitician model)
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

        private void UpdatePerksView(IPolitician model)
        {
            Asserting.NotNull(perkNameLabel, perkLvl1Requirement, perkLvl1Effect, perkLvl2Requirement, perkLvl2Effect,
                perkLvl3Requirement, perkLvl3Effect);

            perkNameLabel.text = model.Character.PerkLvl1.Caption;

            perkLvl1Requirement.text = model.Character.PerkLvl1.Requirement;
            perkLvl1Effect.text = model.Character.PerkLvl1.Effect;
            perkLvl2Requirement.text = model.Character.PerkLvl2.Requirement;
            perkLvl2Effect.text = model.Character.PerkLvl2.Effect;
            perkLvl3Requirement.text = model.Character.PerkLvl3.Requirement;
            perkLvl3Effect.text = model.Character.PerkLvl3.Effect;
        }

        private bool IsModelValid([NotNullWhen(true)] out IPolitician? model)
        {
            model = _model;
            return _model != null;
        }
    }
}