using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.AI.Tactical;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Presentation.Common.Utils;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionBriefingController : MonoBehaviour
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private MissionsManager missionsManager;
        [SerializeField] private Planet[] defaultPlanets;
        [SerializeField] private SimpleAI[] enemyAi;
        [SerializeField] private TextMeshProUGUI countdownText;

        private const int CountDownStart = 3;

        private void Start()
        {
            StartCoroutine(RunSingleRun().ToCoroutine());
        }

        private async UniTask RunSingleRun()
        {
            var winner = missionsManager.LastWinner?.ToString();
            for (int i = CountDownStart; i >= 0; i--)
            {
                countdownText.text = winner != null
                    ? winner + Environment.NewLine + StringBag.FromInt(i)
                    : StringBag.FromInt(i);

                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            var system = pool.GetObject<PlanetarySystem, PlanetarySystem.State>(new PlanetarySystem.State(0.3f));
            foreach (var planet in defaultPlanets)
                system.AddPlanet(planet);

            await missionsManager.NextMission(system, enemyAi);
        }
    }
}