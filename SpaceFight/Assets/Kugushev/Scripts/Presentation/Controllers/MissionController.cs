using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private float productionTimeoutSeconds;
        [SerializeField] private MissionsManager missionsManager;

        private WaitForSeconds _productionTimeout;

        private void Awake()
        {
            _productionTimeout = new WaitForSeconds(productionTimeoutSeconds);
        }

        private void Start()
        {
            StartCoroutine(ProductionCycle());
            StartCoroutine(StatusCheck());
        }

        private IEnumerator ProductionCycle()
        {
            while (true)
            {
                if (missionsManager.CurrentPlanetarySystem != null)
                {
                    foreach (var planet in missionsManager.CurrentPlanetarySystem.Planets)
                    {
                        var task = planet.ExecuteProductionCycle();
                        yield return task.ToCoroutine();
                    }

                    yield return _productionTimeout;
                }
            }
        }

        private IEnumerator StatusCheck()
        {
            while (true)
            {
                yield return missionsManager.CheckMissionStatus().ToCoroutine();
            }
        }
    }
}