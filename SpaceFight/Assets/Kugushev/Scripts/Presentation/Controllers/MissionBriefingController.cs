using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.AI.Tactical;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Player;
using Kugushev.Scripts.Mission.ProceduralGeneration;
using Kugushev.Scripts.Mission.ValueObjects;
using Kugushev.Scripts.Presentation.Common.Utils;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionBriefingController : MonoBehaviour
    {
        [SerializeField] private MissionManagerOld missionManager;

        [Header("Menu")] [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private int countDownStart = 5;

        [Header("Planetary System")] [SerializeField]
        private PlanetarySystemGenerator planetarySystemGenerator;

        [SerializeField] private int seed;

        [Header("Mission Related Assets")] [SerializeField]
        private PlayerCommander playerCommander;

        [SerializeField] private SimpleAI enemyAi;
        [SerializeField] private Fleet greenFleet;
        [SerializeField] private Fleet redFleet;

        private void Start()
        {
            StartCoroutine(RunSingleRun().ToCoroutine());
        }

        private async UniTask RunSingleRun()
        {
            var summary = $"You vs AI" + Environment.NewLine + $"{missionManager.Wins}:{missionManager.Looses}";
            for (int i = countDownStart; i >= 0; i--)
            {
                countdownText.text = summary + Environment.NewLine + StringBag.FromInt(i);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            if (seed == default)
                seed = DateTime.UtcNow.Millisecond;

            var planetarySystem = planetarySystemGenerator.CreatePlanetarySystem(seed);

            var green = new ConflictParty(Faction.Green, greenFleet, playerCommander);
            var red = new ConflictParty(Faction.Red, redFleet, enemyAi);

            await missionManager.NextMission(planetarySystem, green, red, true);
        }
    }
}