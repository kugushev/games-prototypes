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
        [SerializeField] private float unitSize = 2f;

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
            int spawningUnits = 1;
            if (_count >= 2)
                spawningUnits = 2;
            if (_count >= 6)
                spawningUnits = 4;
            if (_count >= 18)
                spawningUnits = 8;
            if (_count >= 32)
                spawningUnits = 8;
            if (_count >= 48)
                spawningUnits = 16;

            var spawnPoint = new Vector3(
                -1 * (spawningUnits / 2) * unitSize,
                _currentSpawn.y,
                _currentSpawn.z + unitSize
            );

            int big = Random.Range(0, spawningUnits * 2);
            for (int i = 0; i < spawningUnits; i++)
            {
                var go = Instantiate(prefab, spawnPoint, Quaternion.Euler(0, 180, 0));
                if (i != big || _count < 10)
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