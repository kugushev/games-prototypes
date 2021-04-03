using System;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Models
{
    public class Politician : Poolable<Politician.State>
    {
        public struct State
        {
            public readonly PoliticianCharacter Character;
            public readonly Percentage IncomeProbability;
            public int RelationLevel;
            public int Budget;
            public Traits Traits;
            public TraitsStatus TraitsStatus;

            public State(PoliticianCharacter character, Percentage incomeProbability, int startBudget, Traits traits)
            {
                Character = character;
                IncomeProbability = incomeProbability;
                Budget = startBudget;
                Traits = traits;
                RelationLevel = GameConstants.StartRelationLevel;
                TraitsStatus = new TraitsStatus();
            }
        }

        public Politician(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public PoliticianCharacter Character => ObjectState.Character;

        public int RelationLevel => ObjectState.RelationLevel;
        public Relation Relation => RelationsService.FromLevel(ObjectState.RelationLevel);

        public int Budget => ObjectState.Budget;

        public bool IsReadyToInvest => ObjectState.Budget > 0 &&
                                       (Relation == Relation.Partner || Relation == Relation.Loyalist);

        public Traits Traits => ObjectState.Traits;
        public TraitsStatus TraitsStatus => ObjectState.TraitsStatus;

        public void ApplyPoliticalAction(PoliticalAction politicalAction)
        {
            var intrigueTraits = politicalAction.Traits;

            if (politicalAction.Intel > 0)
                ObjectState.TraitsStatus =
                    ObjectState.TraitsStatus.RevealOne(ObjectState.Traits, politicalAction.Intel);

            ApplyTraitsEffect(intrigueTraits);
        }

        private void ApplyTraitsEffect(Traits traits)
        {
            int relationChange = 0;
            relationChange += ObjectState.Traits.Business * traits.Business;
            relationChange += ObjectState.Traits.Greed * traits.Greed;
            relationChange += ObjectState.Traits.Lust * traits.Lust;
            relationChange += ObjectState.Traits.Brute * traits.Brute;
            relationChange += ObjectState.Traits.Vanity * traits.Vanity;

            ObjectState.RelationLevel += relationChange;

            if (ObjectState.RelationLevel < GameConstants.MinRelationLevel)
                ObjectState.RelationLevel = GameConstants.MinRelationLevel;
            if (ObjectState.RelationLevel > GameConstants.MaxRelationLevel)
                ObjectState.RelationLevel = GameConstants.MaxRelationLevel;
        }

        public void ApplyIncome()
        {
            if (ObjectState.Budget <= 0)
            {
                ObjectState.Budget += GameConstants.PoliticianIncome;
                return;
            }

            var range = Random.Range(0f, 1f);
            if (range > ObjectState.IncomeProbability.Amount)
            {
                ObjectState.Budget = Mathf.Min(
                    ObjectState.Budget + GameConstants.PoliticianIncome,
                    GameConstants.MaxBudget);
            }
        }

        public void CollectMoney()
        {
            ObjectState.Budget = 0;
        }
    }
}