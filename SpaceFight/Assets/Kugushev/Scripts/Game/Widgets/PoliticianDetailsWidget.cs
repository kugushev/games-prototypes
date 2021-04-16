using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticianDetailsWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameLabel;

        [Header("Relation")] [SerializeField] private TextMeshProUGUI relationValueLabel;
        [SerializeField] private TextMeshProUGUI relationTip;
        [SerializeField] private GameObject relationEnemy;
        [SerializeField] private GameObject relationHater;
        [SerializeField] private GameObject relationIndifferent;
        [SerializeField] private GameObject relationPartner;
        [SerializeField] private GameObject relationLoyalist;

        [Header("Budget")] [SerializeField] private TextMeshProUGUI budgetValueLabel;
        [SerializeField] private GameObject budgetIsReadyToInvest;
        [SerializeField] private GameObject budgetIsNotReadyToInvest;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI traitBusinessValue;
        [SerializeField] private TextMeshProUGUI traitGreedValue;
        [SerializeField] private TextMeshProUGUI traitLustValue;
        [SerializeField] private TextMeshProUGUI traitBruteValue;
        [SerializeField] private TextMeshProUGUI traitVanityValue;

        [Header("Perks")] [SerializeField] private TextMeshProUGUI perkNameLabel;
        [SerializeField] private TextMeshProUGUI perkLvl1Requirement;
        [SerializeField] private TextMeshProUGUI perkLvl1Effect;
        [SerializeField] private TextMeshProUGUI perkLvl2Requirement;
        [SerializeField] private TextMeshProUGUI perkLvl2Effect;
        [SerializeField] private TextMeshProUGUI perkLvl3Requirement;
        [SerializeField] private TextMeshProUGUI perkLvl3Effect;

        const string UnknownTrait = "?";

        private Politician _model;
        
        public void Select(Politician model)
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
            if (!IsModelValid())
                return;

            nameLabel.text = _model.Character.FullName;
            UpdateRelationView();
            UpdateBudgetView();
            UpdateTraitsView();
            UpdatePerksView();
        }

        private void UpdateRelationView()
        {
            relationValueLabel.text = StringBag.FromInt(_model.RelationLevel);

            relationEnemy.SetActive(false);
            relationHater.SetActive(false);
            relationIndifferent.SetActive(false);
            relationPartner.SetActive(false);
            relationLoyalist.SetActive(false);

            switch (_model.Relation)
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
                    Debug.LogError($"Unexpected relation {_model.Relation}");
                    break;
            }
        }

        private void UpdateBudgetView()
        {
            budgetValueLabel.text = StringBag.FromInt(_model.Budget);
            budgetIsReadyToInvest.SetActive(_model.IsReadyToInvest);
            budgetIsNotReadyToInvest.SetActive(!_model.IsReadyToInvest);
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

        private void UpdatePerksView()
        {
            perkNameLabel.text = _model.Character.PerkLvl1.Caption;

            perkLvl1Requirement.text = _model.Character.PerkLvl1.Requirement;
            perkLvl1Effect.text = _model.Character.PerkLvl1.Effect;
            perkLvl2Requirement.text = _model.Character.PerkLvl2.Requirement;
            perkLvl2Effect.text = _model.Character.PerkLvl2.Effect;
            perkLvl3Requirement.text = _model.Character.PerkLvl3.Requirement;
            perkLvl3Effect.text = _model.Character.PerkLvl3.Effect;
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