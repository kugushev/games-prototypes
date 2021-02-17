using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Missions;
using Kugushev.Scripts.Game.Missions.AI.Tactical;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Game.Missions.Player;
using Kugushev.Scripts.Game.Missions.Presets;
using Kugushev.Scripts.Game.Missions.ValueObjects;
using Kugushev.Scripts.Presentation.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionBriefingController : MonoBehaviour
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private MissionManager missionManager;
        [Header("Menu")] [SerializeField] private TextMeshProUGUI countdownText;

        [Header("Planetary System")] [SerializeField]
        private PlanetPreset[] defaultPlanets;

        [SerializeField] private SunPreset sunPreset;

        [Header("Mission Related Assets")] [SerializeField]
        private PlayerCommander playerCommander;

        [SerializeField] private SimpleAI enemyAi;
        [SerializeField] private SimpleAI alternativeAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        private const int CountDownStart = 3;

        // todo: use a dedicated controller for testing purposes
        public static bool MissionFinished { get; private set; }

        private void Start()
        {
            StartCoroutine(RunSingleRun().ToCoroutine());
        }

        private async UniTask RunSingleRun()
        {
            missionManager.MissionFinished += _ => MissionFinished = true;

            var winner = missionManager.LastWinner?.ToString();
            for (int i = CountDownStart; i >= 0; i--)
            {
                countdownText.text = winner != null
                    ? winner + Environment.NewLine + StringBag.FromInt(i)
                    : StringBag.FromInt(i);

                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            var system = pool.GetObject<PlanetarySystem, PlanetarySystem.State>(
                new PlanetarySystem.State(sunPreset.ToSun()));

            foreach (var planetPreset in defaultPlanets)
                system.AddPlanet(planetPreset.ToPlanet(pool));

            ICommander greenCommander = (ICommander) playerCommander ?? alternativeAi;
            var green = new ConflictParty(Faction.Green, greenFleet, greenCommander);

            var red = new ConflictParty(Faction.Red, redFleet, enemyAi);

            await missionManager.NextMission(system, green, red);
        }
    }
}