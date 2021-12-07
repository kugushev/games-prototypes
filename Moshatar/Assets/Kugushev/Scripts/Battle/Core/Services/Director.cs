using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Services
{
    public class Director : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        private readonly (float time, int max)[] _map = {
            (0f, 0),
            (8.654f, 2),
            (24.976f, 6),
            (60f, 0),
            (60f + 25.061f, 12),
            (60f + 41.853f, 6),
            (60f + 57.088f, 0),
            (120f + 3.4f, 6),
            (120f + 28.733f, 12),
            (120f + 45.638f, 0),
            (120f + 49.606f, 20),
            (180f + 4.728f, 0),
            (180f + 15.568f, 2),
            (180f + 23.034f, 12),
            (180f + 41.261f, 20),
            (180f + 54.489f, 0)
        };

        public int GetMax()
        {
            var currentTime = source.time;
            var lastMax = 0;
            for (int i = 0; i < _map.Length; i++)
            {
                var (time, max) = _map[i];
                if (currentTime < time)
                    return lastMax;
                lastMax = max;
            }

            return 0;
        }
    }
}