using System;
using System.Collections;
using System.Linq;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Core.Services;
using TMPro;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC.Benchmark
{
    public class BenchmarkManager : MonoBehaviour
    {
        [SerializeField] private Button spawn;
        [SerializeField] private GameObject prefab;
        [SerializeField] private TextMeshProUGUI units;
        [SerializeField] private Button back;
        [SerializeField] private float scale = 1f;
        
        [Inject] private GameModeService _gameModeService;

        private Vector3 _currentSpawn = Vector3.zero;
        private int _count = 0;

        private void Awake()
        {
            Performance.TryGetAvailableDisplayRefreshRates(out var rates);
            units.text = string.Join(", ", rates);

            if (rates.Length > 0)
            {
                var result = Performance.TrySetDisplayRefreshRate(rates.Max());
                if (!result)
                    units.color = Color.red;
            }
            else
                units.color = Color.magenta;


            spawn.onClick.AddListener(() => StartCoroutine(SpawnNext()));
            back.onClick.AddListener(() => _gameModeService.BackToMenu());
        }

        private IEnumerator SpawnNext()
        {
            const int spawningUnits = 16;
            const float unitSize = 2f;

            var spawnPoint = new Vector3(
                -1 * (spawningUnits / 2) * unitSize,
                _currentSpawn.y,
                _currentSpawn.z + unitSize
            );

            int big = Random.Range(0, spawningUnits * 2);
            for (int i = 0; i < spawningUnits; i++)
            {
                var go = Instantiate(prefab, spawnPoint, Quaternion.Euler(0, 180, 0));
                if (i != big)
                    go.transform.localScale = new Vector3(scale, scale, scale);
                else
                    go.transform.localScale = new Vector3(scale * 2, scale * 2, scale * 2);

                spawnPoint.x += unitSize;
                yield return null;
            }

            _currentSpawn = spawnPoint;
            _count += spawningUnits;
            units.text = StringBag.FromInt(_count);
        }
    }
}