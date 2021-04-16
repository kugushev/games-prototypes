using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Game.Utils;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    public class CampaignFinalizationState : IState
    {
        private CampaignResult? _campaignResult;
        private CampaignSceneResultPipeline? _campaignSceneResultPipeline;

        public void Setup(CampaignResult campaignResult,
            CampaignSceneResultPipeline? campaignSceneResultPipeline)
        {
            _campaignResult = campaignResult;
            _campaignSceneResultPipeline = campaignSceneResultPipeline;
        }

        public UniTask OnEnterAsync()
        {
            Asserting.NotNull(_campaignResult);
            
            if (_campaignSceneResultPipeline != null)
                _campaignSceneResultPipeline.Set(new CampaignResultInfo(_campaignResult));
            else
                _campaignResult.Dispose();

            return UniTask.CompletedTask;
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public UniTask OnExitAsync()
        {
            _campaignResult = null;
            _campaignSceneResultPipeline = null;
            return UniTask.CompletedTask;
        }
    }
}