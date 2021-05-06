using System;
using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Politics.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.Widgets
{
    public class PoliticianDetailsWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameLabel = default!;

        [Header("Relation")] [SerializeField] private TextMeshProUGUI relationValueLabel = default!;
        [SerializeField] private TextMeshProUGUI relationTip = default!;
        [SerializeField] private GameObject relationEnemy = default!;
        [SerializeField] private GameObject relationHater = default!;
        [SerializeField] private GameObject relationIndifferent = default!;
        [SerializeField] private GameObject relationPartner = default!;
        [SerializeField] private GameObject relationLoyalist = default!;

        [Header("Budget")] [SerializeField] private TextMeshProUGUI budgetValueLabel = default!;
        [SerializeField] private GameObject budgetIsReadyToInvest = default!;
        [SerializeField] private GameObject budgetIsNotReadyToInvest = default!;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI traitBusinessValue = default!;
        [SerializeField] private TextMeshProUGUI traitGreedValue = default!;
        [SerializeField] private TextMeshProUGUI traitLustValue = default!;
        [SerializeField] private TextMeshProUGUI traitBruteValue = default!;
        [SerializeField] private TextMeshProUGUI traitVanityValue = default!;

        [Header("Perks")] [SerializeField] private TextMeshProUGUI perkNameLabel = default!;
        [SerializeField] private TextMeshProUGUI perkLvl1Requirement = default!;
        [SerializeField] private TextMeshProUGUI perkLvl1Effect = default!;
        [SerializeField] private TextMeshProUGUI perkLvl2Requirement = default!;
        [SerializeField] private TextMeshProUGUI perkLvl2Effect = default!;
        [SerializeField] private TextMeshProUGUI perkLvl3Requirement = default!;
        [SerializeField] private TextMeshProUGUI perkLvl3Effect = default!;

        const string UnknownTrait = "?";

        private readonly CompositeDisposable _bindings = new CompositeDisposable();

        [Inject]
        private void Init(IPoliticianSelector politicianSelector)
        {
            politicianSelector.SelectedPolitician.Where(p => p != null).Subscribe(OnSelected!);
            politicianSelector.SelectedPolitician.Where(p => p == null).Subscribe(_ => OnDeselected());
        }

        private void OnSelected(IPolitician model)
        {
            gameObject.SetActive(true);
            BindView(model);
        }

        private void OnDeselected()
        {
            _bindings.Clear();
            gameObject.SetActive(false);
        }

        private void BindView(IPolitician model)
        {
            nameLabel.text = model.Character.FullName;
            BindRelationView(model);
            BindBudgetView(model);
            BindTraitsView(model);
            BindPerksView(model);
        }

        private void BindRelationView(IPolitician model)
        {
            model.RelationLevel
                .Select(StringBag.FromInt)
                .SubscribeToTextMeshPro(relationValueLabel)
                .AddTo(_bindings);

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

        private void BindBudgetView(IPolitician model)
        {
            budgetValueLabel.text = StringBag.FromInt(model.Budget);
            budgetIsReadyToInvest.SetActive(model.IsReadyToInvest);
            budgetIsNotReadyToInvest.SetActive(!model.IsReadyToInvest);
        }

        private void BindTraitsView(IPolitician model)
        {
            UpdateTraitView(traitBusinessValue, model.TraitsStatus.Business, model.Traits.Business);
            UpdateTraitView(traitGreedValue, model.TraitsStatus.Greed, model.Traits.Greed);
            UpdateTraitView(traitLustValue, model.TraitsStatus.Lust, model.Traits.Lust);
            UpdateTraitView(traitBruteValue, model.TraitsStatus.Brute, model.Traits.Brute);
            UpdateTraitView(traitVanityValue, model.TraitsStatus.Vanity, model.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, bool revealed, int value) =>
                label.text = revealed ? StringBag.FromInt(value) : UnknownTrait;
        }

        private void BindPerksView(IPolitician model)
        {
            perkNameLabel.text = model.Character.PerkLvl1.Caption;

            perkLvl1Requirement.text = model.Character.PerkLvl1.Requirement;
            perkLvl1Effect.text = model.Character.PerkLvl1.Effect;
            perkLvl2Requirement.text = model.Character.PerkLvl2.Requirement;
            perkLvl2Effect.text = model.Character.PerkLvl2.Effect;
            perkLvl3Requirement.text = model.Character.PerkLvl3.Requirement;
            perkLvl3Effect.text = model.Character.PerkLvl3.Effect;
        }
    }
}