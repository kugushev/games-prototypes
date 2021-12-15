using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Battle.Core.Services;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Core.Services
{
    public class GameModeService
    {
        private readonly BattleGameplayManager _battleGameplayManager;

        public GameModeService(BattleGameplayManager battleGameplayManager)
        {
            _battleGameplayManager = battleGameplayManager;
        }
        
        public async UniTask StartFight() => await SceneManager.LoadSceneAsync("FightScene");
        public async UniTask StartBenchmark() => await SceneManager.LoadSceneAsync("BenchmarkScene");
        public async UniTask StartDuel() => await SceneManager.LoadSceneAsync("DuelScene");
        public async UniTask BackToMenu() => await SceneManager.LoadSceneAsync("MainMenu");
        public async UniTask StartBattleMusou()
        {
            _battleGameplayManager.SetMusou();
            await SceneManager.LoadSceneAsync("BattleScene");
        }
        
        public async UniTask StartBattleTog()
        {
            _battleGameplayManager.SetTog();
            await SceneManager.LoadSceneAsync("BattleScene");
        }
    }
}