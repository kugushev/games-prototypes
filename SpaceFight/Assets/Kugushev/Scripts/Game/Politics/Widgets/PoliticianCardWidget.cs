using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Politics.Events;
using TMPro;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Game.Politics.Widgets
{
    public class PoliticianCardWidget : MonoBehaviour
    {
        [SerializeField] private PoliticianSelectedEvent onSelected = default!;

        [SerializeField] private TextMeshProUGUI budgetValueLabel = default!;

        [Header("Character")] [SerializeField] private TextMeshProUGUI nameLabel = default!;
        [SerializeField] private TextMeshProUGUI perkNameLabel = default!;

        [Header("Relation")] [SerializeField] private GameObject relationEnemy = default!;
        [SerializeField] private GameObject relationHater = default!;
        [SerializeField] private GameObject relationIndifferent = default!;
        [SerializeField] private GameObject relationPartner = default!;
        [SerializeField] private GameObject relationLoyalist = default!;

        [Header("Traits")] [SerializeField] private TextMeshProUGUI traitBusinessValue = default!;
        [SerializeField] private TextMeshProUGUI traitGreedValue = default!;
        [SerializeField] private TextMeshProUGUI traitLustValue = default!;
        [SerializeField] private TextMeshProUGUI traitBruteValue = default!;
        [SerializeField] private TextMeshProUGUI traitVanityValue = default!;

        const string UnknownTrait = "?";

        private IPolitician? _model;

        public void SetUp(IPolitician model)
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
            budgetValueLabel.text = StringBag.FromInt(model.Budget.Value);
            perkNameLabel.text = model.Character.PerkLvl1.Caption;

            model.Relation.Subscribe(UpdateRelationView);
            UpdateTraitsView(model);
        }

        private void UpdateRelationView(Relation relation)
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
                    Debug.LogError($"Unexpected relation {relation}");
                    break;
            }
        }

        private void UpdateTraitsView(IPolitician model)
        {
            Asserting.NotNull(traitBusinessValue, traitGreedValue, traitLustValue, traitBruteValue, traitVanityValue);

            UpdateTraitView(traitBusinessValue, model.TraitsStatus.Value.Business, model.Traits.Business);
            UpdateTraitView(traitGreedValue, model.TraitsStatus.Value.Greed, model.Traits.Greed);
            UpdateTraitView(traitLustValue, model.TraitsStatus.Value.Lust, model.Traits.Lust);
            UpdateTraitView(traitBruteValue, model.TraitsStatus.Value.Brute, model.Traits.Brute);
            UpdateTraitView(traitVanityValue, model.TraitsStatus.Value.Vanity, model.Traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, bool revealed, int value) =>
                label.text = revealed ? StringBag.FromInt(value) : UnknownTrait;
        }

        private bool IsModelValid([NotNullWhen(true)] out IPolitician? model)
        {
            model = _model;
            return _model != null;
        }
    }
}