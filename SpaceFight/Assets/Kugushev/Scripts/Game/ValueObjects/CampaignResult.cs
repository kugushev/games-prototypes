using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public class CampaignResult : Poolable<CampaignResult.State>
    {
        public struct State
        {
        }

        public CampaignResult(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        protected override void OnRestore(State state) => _rewardedPoliticalActions.Clear();

        private readonly List<PoliticalAction> _rewardedPoliticalActions = new List<PoliticalAction>(16);

        public List<PoliticalAction> RewardedPoliticalActions => _rewardedPoliticalActions;
        public void AddReward(PoliticalAction politicalAction) => _rewardedPoliticalActions.Add(politicalAction);
        protected override void OnClear(State state) => _rewardedPoliticalActions.Clear();
    }

    public readonly struct CampaignResultInfo
    {
        public CampaignResultInfo(CampaignResult campaignResult)
        {
            CampaignResult = campaignResult;
        }

        public CampaignResult CampaignResult { get; }
    }
}