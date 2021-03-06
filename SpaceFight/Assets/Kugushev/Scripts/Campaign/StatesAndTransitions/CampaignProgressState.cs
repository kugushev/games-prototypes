using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Campaign.StatesAndTransitions
{
    internal class CampaignProgressState : BaseState<CampaignModel>
    {
        private const string SceneName = "CampaignProgressScene";

        public CampaignProgressState(CampaignModel model) : base(model)
        {
        }

        public override async UniTask OnEnterAsync()
        {
            await SceneManagerHelper.LoadAndSetActiveAsync(SceneName);
        }

        public override async UniTask OnExitAsync()
        {
            await SceneManager.UnloadSceneAsync(SceneName);
        }
    }
}