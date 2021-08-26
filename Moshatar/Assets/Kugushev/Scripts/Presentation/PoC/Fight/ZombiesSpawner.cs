using System;
using System.Collections;
using Kugushev.Scripts.Presentation.PoC.Music;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC.Fight
{
    public class ZombiesSpawner : MonoBehaviour
    {
        private const float LoudTime = 120f;
        private const float EnrageTime = 195f;
        private const float FinalTime = 240f;

        [SerializeField] private float fixedTimeoutSeconds;
        [SerializeField] private float maxRandomTimeoutSeconds;

        [Inject] private ZombieView.Factory _zombieViewFactory;
        [Inject] private GameDirector _director;

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
                bool isSuitableSection = _director.CurrentSectionType == SongSectionType.Battle ||
                                         _director.CurrentSectionType == SongSectionType.Menace;

                if (isSuitableSection && _currentZombie is null)
                    _zombieViewFactory.Create(transform.position, this);

                // yield return new WaitForSeconds(Random.Range(0, 10));
                yield return new WaitForSeconds(GetTimeout(started));
            }
        }

        private float GetTimeout(DateTime started)
        {
            float elapsed = Convert.ToSingle((DateTime.Now - started).TotalSeconds);

            float timeout;
            if (elapsed < LoudTime)
            {
                float random = Random.Range(0, maxRandomTimeoutSeconds);
                timeout = fixedTimeoutSeconds + random;

                timeout = FitToStage(elapsed, timeout, EnrageTime);
            }
            else if (elapsed < EnrageTime)
            {
                float random = Random.Range(0, maxRandomTimeoutSeconds);
                timeout = fixedTimeoutSeconds * 2 + random;

                timeout = FitToStage(elapsed, timeout, FinalTime);
            }
            else if (elapsed < FinalTime)
            {
                timeout = fixedTimeoutSeconds / 2;
            }
            else
            {
                float random = Random.Range(0, maxRandomTimeoutSeconds);
                timeout = fixedTimeoutSeconds + random;
            }

            return timeout;
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

        public void Release(ZombieView zombieView)
        {
            if (_currentZombie != zombieView)
                throw new Exception("Unexpected zombie");
            _currentZombie = null;
        }
    }
}