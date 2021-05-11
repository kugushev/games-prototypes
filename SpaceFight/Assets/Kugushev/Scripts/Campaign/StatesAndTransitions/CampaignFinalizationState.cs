using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    public class CampaignFinalizationState : IUnparameterizedState
    {
        private CampaignResult? _campaignResult;

        public void Setup(CampaignResult campaignResult)
        {
            _campaignResult = campaignResult;
        }

        public UniTask OnEnterAsync()
        {
            Asserting.NotNull(_campaignResult);
            
            // if (_campaignSceneResultPipeline != null)
            //     _campaignSceneResultPipeline.Set(new CampaignResultInfo(_campaignResult));
            // else
                _campaignResult.Dispose();

            return UniTask.CompletedTask;
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public UniTask OnExitAsync()
        {
            _campaignResult = null;
            return UniTask.CompletedTask;
        }
    }
}