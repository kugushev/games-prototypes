using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MainMenuController: MonoBehaviour
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private MissionsManager missionsManager;
        [SerializeField] private Planet[] defaultPlanets;

        private void Start()
        {
            StartCoroutine(RunSingleRun().ToCoroutine());
        }

        private async UniTask RunSingleRun()
        {
            for (int i = 2; i >= 0; i--)
            {
                print(i);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            var system = pool.GetObject<PlanetarySystem, PlanetarySystem.State>(new PlanetarySystem.State(0.3f));
            foreach (var planet in defaultPlanets) 
                system.AddPlanet(planet);
            
            await missionsManager.NextMission(system);
        }
    }
}