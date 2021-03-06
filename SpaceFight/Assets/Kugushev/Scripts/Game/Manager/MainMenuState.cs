using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Manager
{
    public class MainMenuState: BaseState<GameModel>
    {
        public MainMenuState(GameModel model) : base(model)
        {
        }

        public override bool CanEnter => Model.CurrentCampaign == null;

        public override async UniTask OnEnterAsync()
        {
            await SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName("MainMenuScene");
            bool loaded = SceneManager.SetActiveScene(scene);
            if (!loaded)
            {
                Debug.LogError("Main Menu Scene isn't loaded");
            }
        }
    }
}