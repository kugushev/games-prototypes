using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Game.Core.ValueObjects
{
    public class CampaignResult : PoolableOld<CampaignResult.State>
    {
        public struct State
        {
        }

        public CampaignResult(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        protected override void OnRestore(State state) => _rewardedPoliticalActions.Clear();

        private readonly List<Intrigue> _rewardedPoliticalActions = new List<Intrigue>(16);

        public List<Intrigue> RewardedPoliticalActions => _rewardedPoliticalActions;
        public void AddReward(Intrigue intrigue) => _rewardedPoliticalActions.Add(intrigue);
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