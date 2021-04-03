using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.Models
{
    public class GameModel : Poolable<GameModel.State>
    {
        public readonly struct State
        {
            public readonly Parliament Parliament;

            public readonly CampaignPreparation CampaignPreparation;

            public State(Parliament parliament, CampaignPreparation campaignPreparation)
            {
                Parliament = parliament;
                CampaignPreparation = campaignPreparation;
            }
        }

        private readonly List<PoliticalAction> _politicalActions = new List<PoliticalAction>(64);


        public GameModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public void PrepareNextRound()
        {
            ObjectState.CampaignPreparation.RemoveAllSponsors();
            foreach (var politician in Parliament.Politicians)
            {
                politician.ApplyIncome();
            }
        }

        public Parliament Parliament => ObjectState.Parliament;
        public CampaignPreparation CampaignPreparation => ObjectState.CampaignPreparation;
        public IReadOnlyList<PoliticalAction> PoliticalActions => _politicalActions;


        public void AddPoliticalActions(IReadOnlyList<PoliticalAction> politicalActions) =>
            _politicalActions.AddRange(politicalActions);

        protected override void OnClear(State state)
        {
            state.Parliament.Dispose();
            state.CampaignPreparation.Dispose();
            _politicalActions.Clear();
        }

        protected override void OnRestore(State state) => _politicalActions.Clear();
    }
}