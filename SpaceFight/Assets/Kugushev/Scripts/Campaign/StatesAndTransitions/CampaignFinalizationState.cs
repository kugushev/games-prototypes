using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Game.Utils;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    public class CampaignFinalizationState : IState
    {
        private CampaignResult _campaignResult;
        [CanBeNull] private CampaignSceneResultPipeline _campaignSceneResultPipeline;

        public void Setup(CampaignResult campaignResult,
            [CanBeNull] CampaignSceneResultPipeline campaignSceneResultPipeline)
        {
            _campaignResult = campaignResult;
            _campaignSceneResultPipeline = campaignSceneResultPipeline;
        }

        public UniTask OnEnterAsync()
        {
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