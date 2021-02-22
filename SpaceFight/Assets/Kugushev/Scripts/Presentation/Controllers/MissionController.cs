using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.Missions;
using Kugushev.Scripts.Game.Missions.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private float productionTimeoutSeconds;

        [FormerlySerializedAs("missionsManager")] [SerializeField]
        private MissionManager missionManager;

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
                if (missionManager.State != null)
                {
                    foreach (var planet in missionManager.State.Value.CurrentPlanetarySystem.Planets)
                    {
                        var task = planet.ExecuteProductionCycle();
                        yield return task.ToCoroutine();
                    }
                }

                yield return _productionTimeout;
            }
        }

        private IEnumerator StatusCheck()
        {
            while (true)
            {
                yield return missionManager.CheckMissionStatus().ToCoroutine();
            }
        }
    }
}