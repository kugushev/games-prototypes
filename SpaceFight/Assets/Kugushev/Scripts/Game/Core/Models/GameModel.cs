using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Models
{
    internal class GameModel : Poolable<GameModel.State>
    {
        internal readonly struct State
        {
            public readonly Parliament Parliament;

            public State(Parliament parliament)
            {
                Parliament = parliament;
            }
        }

        private readonly List<IntrigueCard> _politicalActions = new List<IntrigueCard>(64);

        public GameModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public void PrepareNextRound()
        {
            // ObjectState.CampaignPreparation.RemoveAllSponsors();
            foreach (var politician in Parliament.Politicians)
            {
                // politician.ApplyIncome();
            }
        }

        public Parliament Parliament => ObjectState.Parliament;
        public IReadOnlyList<IntrigueCard> PoliticalActions => _politicalActions;

        public void AddPoliticalActions(IReadOnlyList<Intrigue> politicalActions)
        {
            foreach (var politicalAction in politicalActions)
                _politicalActions.Add(new IntrigueCard(politicalAction));
        }

        public void RemovePoliticalAction(IntrigueCard card) => _politicalActions.Remove(card);

        protected override void OnClear(State state)
        {
            // state.Parliament.Dispose();
            _politicalActions.Clear();
        }

        protected override void OnRestore(State state) => _politicalActions.Clear();
    }
}