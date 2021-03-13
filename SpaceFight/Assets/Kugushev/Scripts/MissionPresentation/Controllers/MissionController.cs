using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.Controllers
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private float productionTimeoutSeconds;
        [SerializeField] private MissionModelProvider missionModelProvider;

        private WaitForSeconds _productionTimeout;

        private void Awake()
        {
            _productionTimeout = new WaitForSeconds(productionTimeoutSeconds);
        }

        private void Start()
        {
            StartCoroutine(ProductionCycle());
        }

        private IEnumerator ProductionCycle()
        {
            while (true)
            {
                if (missionModelProvider.TryGetModel(out var missionModel))
                {
                    foreach (var planet in missionModel.PlanetarySystem.Planets)
                    {
                        var task = planet.ExecuteProductionCycle();
                        yield return task.ToCoroutine();
                    }
                }

                yield return _productionTimeout;
            }

            // ReSharper disable once IteratorNeverReturns
        }
    }
}