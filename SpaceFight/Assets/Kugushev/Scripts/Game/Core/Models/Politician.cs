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
        IReadOnlyReactiveProperty<Relation> Relation { get; }
        IReadOnlyReactiveProperty<int> Budget { get; }
        IReadOnlyReactiveProperty<bool> IsReadyToInvest { get; }
        IReadOnlyReactiveProperty<TraitsStatus> TraitsStatus { get; }
        Traits Traits { get; }
    }

    internal class Politician : IPolitician
    {
        private readonly Percentage _incomeProbability;
        private readonly Traits _traits;
        private readonly ReactiveProperty<TraitsStatus> _traitsStatus;
        private readonly ReactiveProperty<int> _budget;
        private readonly ReactiveProperty<int> _relationLevel;

        public Politician(PoliticianCharacter character, Percentage incomeProbability, int startBudget, Traits traits)
        {
            Character = character;
            _incomeProbability = incomeProbability;
            _traits = traits;
            _traitsStatus = new ReactiveProperty<TraitsStatus>(GameConstants.StartTraitsStatus);
            _budget = new ReactiveProperty<int>(startBudget);
            _relationLevel = new ReactiveProperty<int>(GameConstants.StartRelationLevel);
            Relation = _relationLevel.Select(RelationsService.FromLevel).ToReactiveProperty();
            IsReadyToInvest = _budget.CombineLatest(Relation, IsReadyToInvestImpl).ToReactiveProperty();
        }

        public PoliticianCharacter Character { get; }

        public IReadOnlyReactiveProperty<int> RelationLevel => _relationLevel;
        public IReadOnlyReactiveProperty<Relation> Relation { get; }

        public IReadOnlyReactiveProperty<int> Budget => _budget;

        public IReadOnlyReactiveProperty<bool> IsReadyToInvest { get; }

        public IReadOnlyReactiveProperty<TraitsStatus> TraitsStatus => _traitsStatus;
        public Traits Traits => _traits;

        internal void ApplyIntrigue(Intrigue intrigue)
        {
            var intrigueTraits = intrigue.Traits;

            if (intrigue.Intel > 0)
                _traitsStatus.Value = _traitsStatus.Value.RevealOne(_traits, intrigue.Intel);

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
                _budget.Value = Mathf.Min(
                    _budget.Value + GameConstants.PoliticianIncome,
                    GameConstants.MaxBudget);
            }
        }

        public void CollectMoney() => _budget.Value = 0;

        private static bool IsReadyToInvestImpl(int budget, Relation relation) =>
            budget > 0 && (relation == Enums.Relation.Partner || relation == Enums.Relation.Loyalist);
    }
}