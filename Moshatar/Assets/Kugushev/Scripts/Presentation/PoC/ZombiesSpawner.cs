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

        private ZombieView _currentZombie;

        private void Start()
        {
            StartCoroutine(SpawnZombies());
        }

        private IEnumerator SpawnZombies()
        {
            var started = DateTime.Now;
            while (true)
            {
                if (_currentZombie is null)
                    _zombieViewFactory.Create(transform.position, this);

                float elapsed = Convert.ToSingle((DateTime.Now - started).TotalSeconds);

                float timeout;
                if (elapsed < LoudTime)
                {
                    float random = Random.Range(minRandomTimeoutSeconds, maxRandomTimeoutSeconds);
                    timeout = fixedTimeoutSeconds + random;

                    timeout = FitToStage(elapsed, timeout, EnrageTime);
                }
                else if (elapsed < EnrageTime)
                {
                    float random = Random.Range(minRandomTimeoutSeconds, maxRandomTimeoutSeconds);
                    timeout = fixedTimeoutSeconds * 2 + random;

                    timeout = FitToStage(elapsed, timeout, FinalTime);
                }
                else if (elapsed < FinalTime)
                {
                    timeout = fixedTimeoutSeconds / 2;
                }
                else
                {
                    float random = Random.Range(minRandomTimeoutSeconds, maxRandomTimeoutSeconds);
                    timeout = fixedTimeoutSeconds + random;
                }

                yield return new WaitForSeconds(timeout);
            }
        }

        private static float FitToStage(float elapsed, float timeout, float stage)
        {
            if (elapsed + timeout > stage)
            {
                timeout = stage - elapsed;
            }

            return timeout;
        }

        public void Set(ZombieView zombieView)
        {
            if (_currentZombie is null)
                _currentZombie = zombieView;
            else
                throw new Exception("Zombie is already set");
        }

        private void Release(ZombieView zombieView)
        {
            if (_currentZombie != zombieView)
                throw new Exception("Unexpected zombie");
            _currentZombie = null;
        }
    }
}