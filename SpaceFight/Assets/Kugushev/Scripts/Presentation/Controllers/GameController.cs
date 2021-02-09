using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.Entities;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class GameController: MonoBehaviour
    {
        [SerializeField] private float productionTimeoutSeconds;
        [SerializeField] private Planet[] planets;

        private WaitForSeconds _productionTimeout;

        private void Awake()
        {
            _productionTimeout = new WaitForSeconds(productionTimeoutSeconds);
        }

        private IEnumerator Start()
        {
            while (true)
            {
                foreach (var planet in planets)
                {
                    var task = planet.ExecuteProductionCycle();
                    yield return StartCoroutine(task.ToCoroutine());
                }

                yield return _productionTimeout;
            }
        }
    }
}