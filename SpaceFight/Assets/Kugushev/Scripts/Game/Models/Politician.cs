using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.Models
{
    public class Politician : Poolable<Politician.State>
    {
        public struct State
        {
            public PoliticianCharacter Character;
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
        public bool IsReadyToInvest => ObjectState.Budget > 0;

        public Traits Traits => ObjectState.Traits;
        public TraitsStatus TraitsStatus => ObjectState.TraitsStatus;

        public void ApplyPoliticalAction(PoliticalAction politicalAction)
        {
            var intrigueTraits = politicalAction.Traits;

            if (politicalAction.Intel > 0)
                ObjectState.TraitsStatus =
                    ObjectState.TraitsStatus.RevealOne(ObjectState.Traits, politicalAction.Intel);

            ApplyIntrigueTraits(intrigueTraits);
        }

        public void ApplyIntrigueTraits(Traits intrigueTraits)
        {
            int relationChange = 0;
            relationChange += ObjectState.Traits.Business * intrigueTraits.Business;
            relationChange += ObjectState.Traits.Greed * intrigueTraits.Greed;
            relationChange += ObjectState.Traits.Lust * intrigueTraits.Lust;
            relationChange += ObjectState.Traits.Brute * intrigueTraits.Brute;
            relationChange += ObjectState.Traits.Vanity * intrigueTraits.Vanity;

            ObjectState.RelationLevel += relationChange;

            if (ObjectState.RelationLevel < GameConstants.MinRelationLevel)
                ObjectState.RelationLevel = GameConstants.MinRelationLevel;
            if (ObjectState.RelationLevel > GameConstants.MaxRelationLevel)
                ObjectState.RelationLevel = GameConstants.MaxRelationLevel;
        }
    }
}