﻿using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Core.Services
{
    public class GameModeService
    {
        public async UniTask StartFight() => await SceneManager.LoadSceneAsync("FightScene");
        public async UniTask StartBenchmark() => await SceneManager.LoadSceneAsync("BenchmarkScene");
        public async UniTask BackToMenu() => await SceneManager.LoadSceneAsync("MainMenu");
    }
}