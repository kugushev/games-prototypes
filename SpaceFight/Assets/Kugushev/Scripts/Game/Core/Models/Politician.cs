using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Models
{
    public interface IPolitician
    {
        PoliticianCharacter Character { get; }
        int RelationLevel { get; }
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
        private int _relationLevel;

        public Politician(PoliticianCharacter character, Percentage incomeProbability, int startBudget, Traits traits)
        {
            Character = character;
            _incomeProbability = incomeProbability;
            _traits = traits;
            _traitsStatus = new TraitsStatus();
            _budget = startBudget;
            _relationLevel = GameConstants.StartRelationLevel;
        }

        public PoliticianCharacter Character { get; }

        public int RelationLevel => _relationLevel;
        public Relation Relation => RelationsService.FromLevel(_relationLevel);

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

            _relationLevel += relationChange;

            if (_relationLevel < GameConstants.MinRelationLevel)
                _relationLevel = GameConstants.MinRelationLevel;
            if (_relationLevel > GameConstants.MaxRelationLevel)
                _relationLevel = GameConstants.MaxRelationLevel;
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