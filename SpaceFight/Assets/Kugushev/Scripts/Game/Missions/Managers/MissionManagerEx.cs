using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.FiniteStateMachine;
using UnityEngine.SceneManagement;
using static Kugushev.Scripts.Game.Common.UnityConstants.Scenes;

namespace Kugushev.Scripts.Game.Missions.Managers
{
    public class MissionManagerEx
    {
        private readonly StateMachine _stateMachine = new StateMachine(
            new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                
            });
    }

    public class BriefingState: IState
    {
        public async UniTask OnEnterAsync()
        {
            await SceneManager.LoadSceneAsync(MissionPreparationScene);
        }

        public void OnUpdate(float deltaTime)
        {

        }

        public UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}