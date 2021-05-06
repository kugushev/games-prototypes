using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Services;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Core.Models
{
    public interface IPolitician
    {
        PoliticianCharacter Character { get; }
        IReadOnlyReactiveProperty<int> RelationLevel { get; }
        Relation Relation { get; }
        int Budget { get; }
        bool IsReadyToInvest { get; }
        Traits Traits { get; }
        TraitsStatus TraitsStatus { get; }
    }

    internal class Politician : IPolitician
    {
        private readonly Percentage _incomeProbability;
        private readonly Traits _traits;
        private TraitsStatus _traitsStatus;
        private int _budget;
        private ReactiveProperty<int> _relationLevel;

        public Politician(PoliticianCharacter character, Percentage incomeProbability, int startBudget, Traits traits)
        {
            Character = character;
            _incomeProbability = incomeProbability;
            _traits = traits;
            _traitsStatus = new TraitsStatus();
            _budget = startBudget;
            _relationLevel = new ReactiveProperty<int>(GameConstants.StartRelationLevel);
        }

        public PoliticianCharacter Character { get; }

        public IReadOnlyReactiveProperty<int> RelationLevel => _relationLevel;
        public Relation Relation => RelationsService.FromLevel(_relationLevel.Value);

        public int Budget => _budget;

        public bool IsReadyToInvest => _budget > 0 && (Relation == Relation.Partner || Relation == Relation.Loyalist);

        public Traits Traits => _traits;
        public TraitsStatus TraitsStatus => _traitsStatus;

        internal void ApplyPoliticalAction(Intrigue intrigue)
        {
            var intrigueTraits = intrigue.Traits;

            if (intrigue.Intel > 0)
                _traitsStatus = _traitsStatus.RevealOne(_traits, intrigue.Intel);

            ApplyTraitsEffect(intrigueTraits);
        }

        private void ApplyTraitsEffect(Traits traits)
        {
            int relationChange = 0;
            relationChange += _traits.Business * traits.Business;
            relationChange += _traits.Greed * traits.Greed;
            relationChange += _traits.Lust * traits.Lust;
            relationChange += _traits.Brute * traits.Brute;
            relationChange += _traits.Vanity * traits.Vanity;

            UpdateRelationLevel(relationChange);
        }

        private void UpdateRelationLevel(int relationChange)
        {
            var relationLevel = _relationLevel.Value;
            relationLevel += relationChange;

            if (relationLevel < GameConstants.MinRelationLevel)
                relationLevel = GameConstants.MinRelationLevel;
            if (relationLevel > GameConstants.MaxRelationLevel)
                relationLevel = GameConstants.MaxRelationLevel;
            _relationLevel.Value = relationLevel;
        }

        internal void ApplyIncome()
        {
            var range = Random.Range(0f, 1f);
            if (range > _incomeProbability.Amount)
            {
                _budget = Mathf.Min(
                    _budget + GameConstants.PoliticianIncome,
                    GameConstants.MaxBudget);
            }
        }

        public void CollectMoney() => _budget = 0;
    }
}