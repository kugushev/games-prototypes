using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Game.Politics.Constants;
using Kugushev.Scripts.Game.Politics.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class PoliticianCardPresentationModel : MonoBehaviour
    {
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

        private IPolitician? _model;
        private IParliamentPresentationModel? _root;

        public void SetUp(IPolitician model, IParliamentPresentationModel root)
        {
            _model = model;
            _root = root;
            SetupView(_model);
        }

        public void ToggleChanged(bool isOn)
        {
            if (_model is null || _root is null)
                return;

            _root.SelectPolitician(isOn ? _model : null);
        }

        private void SetupView(IPolitician model)
        {
            nameLabel.text = model.Character.FullName;
            model.Budget.Select(StringBag.FromInt).SubscribeToTextMeshPro(budgetValueLabel).AddTo(this);
            perkNameLabel.text = model.Character.PerkLvl1.Caption;

            model.Relation.Subscribe(UpdateRelationView).AddTo(this);
            model.TraitsStatus.Subscribe(status => UpdateTraitsView(status, model.Traits)).AddTo(this);
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

        private void UpdateTraitsView(TraitsStatus traitsStatus, Traits traits)
        {
            UpdateTraitView(traitBusinessValue, traitsStatus.Business, traits.Business);
            UpdateTraitView(traitGreedValue, traitsStatus.Greed, traits.Greed);
            UpdateTraitView(traitLustValue, traitsStatus.Lust, traits.Lust);
            UpdateTraitView(traitBruteValue, traitsStatus.Brute, traits.Brute);
            UpdateTraitView(traitVanityValue, traitsStatus.Vanity, traits.Vanity);

            void UpdateTraitView(TextMeshProUGUI label, bool revealed, int value) =>
                label.text = revealed ? StringBag.FromInt(value) : PoliticsConstants.UnrevealedTrait;
        }
    }
}