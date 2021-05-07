using System;
using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;
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
            SetupView(model);
        }

        private void OnDeselected()
        {
            _bindings.Clear();
            gameObject.SetActive(false);
        }

        private void SetupView(IPolitician model)
        {
            nameLabel.text = model.Character.FullName;
            BindRelationView(model);
            BindBudgetView(model);
            model.TraitsStatus.Subscribe(status => UpdateTraitsView(status, model.Traits)).AddTo(_bindings);
            UpdatePerksView(model);
        }

        private void BindRelationView(IPolitician model)
        {
            model.RelationLevel
                .Select(StringBag.FromInt)
                .SubscribeToTextMeshPro(relationValueLabel)
                .AddTo(_bindings);

            model.Relation.Subscribe(UpdateRelationView).AddTo(_bindings);

            void UpdateRelationView(Relation relation)
            {
                relationEnemy.SetActive(false);
                relationHater.SetActive(false);
                relationIndifferent.SetActive(false);
                relationPartner.SetActive(false);
                relationLoyalist.SetActive(false);

                switch (relation)
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
        }

        private void BindBudgetView(IPolitician model)
        {
            model.Budget.Select(StringBag.FromInt).SubscribeToTextMeshPro(budgetValueLabel).AddTo(_bindings);

            model.IsReadyToInvest.Subscribe(isReady =>
            {
                budgetIsReadyToInvest.SetActive(isReady);
                budgetIsNotReadyToInvest.SetActive(!isReady);
            }).AddTo(_bindings);
        }

        private void UpdateTraitsView(TraitsStatus traitsStatus, Traits traits)
        {
            UpdateTraitView(traitBusinessValue, traitsStatus.Business, traits.Business);
            UpdateTraitView(traitGreedValue, traitsStatus.Greed, traits.Greed);
            UpdateTraitView(traitLustValue, traitsStatus.Lust, traits.Lust);
            UpdateTraitView(traitBruteValue, traitsStatus.Brute, traits.Brute);
            UpdateTraitView(traitVanityValue, traitsStatus.Vanity, traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, bool revealed, int value) =>
                label.text = revealed ? StringBag.FromInt(value) : UnknownTrait;
        }

        private void UpdatePerksView(IPolitician model)
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