using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class ZombiesSpawner : MonoBehaviour
    {
        private const float LoudTime = 120f;
        private const float EnrageTime = 195f;
        private const float FinalTime = 240f;

        [SerializeField] private float fixedTimeoutSeconds;
        [SerializeField] private float minRandomTimeoutSeconds;
        [SerializeField] private float maxRandomTimeoutSeconds;

        [Inject] private ZombieView.Factory _zombieViewFactory;

        private void Start()
        {
            StartCoroutine(SpawnZombies());
        }

        private IEnumerator SpawnZombies()
        {
            var started = DateTime.Now;
            while (true)
            {
                var elapsed = (DateTime.Now - started).TotalSeconds;

                float timeout;
                if (elapsed < LoudTime)
                {
                    float random = Random.Range(minRandomTimeoutSeconds, maxRandomTimeoutSeconds);
                    timeout = fixedTimeoutSeconds + random;
                }
                else if (elapsed < EnrageTime)
                {
                    float random = Random.Range(minRandomTimeoutSeconds, maxRandomTimeoutSeconds);
                    timeout = 10 + random;
                }
                else if (elapsed < FinalTime)
                {
                    timeout = 1;
                }
                else
                {
                    float random = Random.Range(minRandomTimeoutSeconds, maxRandomTimeoutSeconds);
                    timeout = fixedTimeoutSeconds + random;
                }


                yield return new WaitForSeconds(timeout);


                _zombieViewFactory.Create(transform.position);
            }
        }
    }
}