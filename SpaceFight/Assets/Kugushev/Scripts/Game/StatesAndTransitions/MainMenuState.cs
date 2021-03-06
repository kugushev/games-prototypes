using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Models;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    internal class MainMenuState : BaseState<GameModel>
    {
        const string SceneName = "MainMenuScene";

        public MainMenuState(GameModel model) : base(model)
        {
        }

        public override async UniTask OnEnterAsync()
        {
            await SceneManagerHelper.LoadAndSetActiveAsync(SceneName);
        }

        public override async UniTask OnExitAsync()
        {
            Model.MainMenu.StartClicked = false;
            await SceneManager.UnloadSceneAsync(SceneName);
        }
    }
}