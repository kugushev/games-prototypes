using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Utils;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    internal class CampaignState : BaseState<GameModel>
    {
        private readonly CampaignSceneParametersPipeline _sceneParametersPipeline;
        const string SceneName = "CampaignManagementScene";

        public CampaignState(GameModel model,
            CampaignSceneParametersPipeline sceneParametersPipeline) : base(model)
        {
            _sceneParametersPipeline = sceneParametersPipeline;
        }

        public override async UniTask OnEnterAsync()
        {
            var campaignInfo = new CampaignInfo(Model.MainMenu.Seed);
            _sceneParametersPipeline.Set(campaignInfo);

            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        }

        public override async UniTask OnExitAsync()
        {
            await SceneManager.UnloadSceneAsync(SceneName);
        }
    }
}