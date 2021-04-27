using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.Models
{
    internal class GameModel : Poolable<GameModel.State>
    {
        internal readonly struct State
        {
            public readonly Parliament Parliament;

            public readonly CampaignPreparation CampaignPreparation;

            public State(Parliament parliament, CampaignPreparation campaignPreparation)
            {
                Parliament = parliament;
                CampaignPreparation = campaignPreparation;
            }
        }

        private readonly List<IntrigueRecord> _politicalActions = new List<IntrigueRecord>(64);


        public GameModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public void PrepareNextRound()
        {
            ObjectState.CampaignPreparation.RemoveAllSponsors();
            foreach (var politician in Parliament.Politicians)
            {
                // politician.ApplyIncome();
            }
        }

        public Parliament Parliament => ObjectState.Parliament;
        internal CampaignPreparation CampaignPreparation => ObjectState.CampaignPreparation;
        public IReadOnlyList<IntrigueRecord> PoliticalActions => _politicalActions;

        public void AddPoliticalActions(IReadOnlyList<Intrigue> politicalActions)
        {
            foreach (var politicalAction in politicalActions)
                _politicalActions.Add(new IntrigueRecord(politicalAction));
        }

        public void RemovePoliticalAction(IntrigueRecord @record) => _politicalActions.Remove(record);

        protected override void OnClear(State state)
        {
            // state.Parliament.Dispose();
            state.CampaignPreparation.Dispose();
            _politicalActions.Clear();
        }

        protected override void OnRestore(State state) => _politicalActions.Clear();
    }
}