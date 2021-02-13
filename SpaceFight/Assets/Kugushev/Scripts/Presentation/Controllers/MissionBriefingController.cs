using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Missions;
using Kugushev.Scripts.Game.Missions.AI.Tactical;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Presentation.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionBriefingController : MonoBehaviour
    {
        [SerializeField] private ObjectsPool pool;
        [FormerlySerializedAs("missionsManager")] [SerializeField] private MissionManager missionManager;
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
            var winner = missionManager.LastWinner?.ToString();
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

            await missionManager.NextMission(system, enemyAi);
        }
    }
}