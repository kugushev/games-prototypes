﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.Services
{
    public class Director : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [Inject] private BattleGameplayManager _gameplayManager;

        private (float time, int max, bool spawnBig, int maxBig)[] _map;

        public int? MaxOverride { get; set; }

        // private readonly (float time, int max, bool spawnBig, int maxBig)[] _map =
        // {
        //     (0f, 0, false, 0),
        //     (8.654f, 8, false, 0),
        //     (24.976f, 8, true, 1),
        //     (60f, 0, false, 0),
        //     (60f + 25.061f, 12, true, 3),
        //     (60f + 41.853f, 8, false, 0),
        //     (60f + 57.088f, 0, false, 0),
        //     (120f + 3.4f, 8, false, 0),
        //     (120f + 28.733f, 12, true, 3),
        //     (120f + 45.638f, 0, false, 0),
        //     (120f + 49.606f, 12, true, 5),
        //     (180f + 4.728f, 0, false, 0),
        //     (180f + 15.568f, 8, false, 0),
        //     (180f + 23.034f, 12, true, 3),
        //     (180f + 41.261f, 12, true, 5),
        //     (180f + 54.489f, 0, false, 0)
        // };

        private int[] _bigSpawnedPerStage;

        // private readonly (float time, int max)[] _map = {
        //     (0f, 0),
        //     (8.654f, 2),
        //     (24.976f, 6),
        //     (60f, 0),
        //     (60f + 25.061f, 12),
        //     (60f + 41.853f, 6),
        //     (60f + 57.088f, 0),
        //     (120f + 3.4f, 6),
        //     (120f + 28.733f, 12),
        //     (120f + 45.638f, 0),
        //     (120f + 49.606f, 20),
        //     (180f + 4.728f, 0),
        //     (180f + 15.568f, 2),
        //     (180f + 23.034f, 12),
        //     (180f + 41.261f, 20),
        //     (180f + 54.489f, 0)
        // };


        public void RegisterBigSpawning(int idx) => _bigSpawnedPerStage[idx] += 1;


        public (int max, bool spawnBig, int index) GetMax()
        {
            if (MaxOverride != null)
                return (MaxOverride.Value, false, 0);

            _map ??= InitMap();
            _bigSpawnedPerStage ??= new int[_map.Length];

            var currentTime = source.time;
            for (int i = 0; i < _map.Length; i++)
            {
                var (time, _, _, _) = _map[i];
                if (currentTime < time)
                {
                    // we need to get PREVIOUS stage because we already jumped it over
                    var idx = Mathf.Max(i - 1, 0);

                    return GetMaxByIndex(idx);
                }
            }

            return (0, false, 0);
        }

        private (int max, bool spawnBig, int index) GetMaxByIndex(int idx)
        {
            var (_, max, spawnBig, maxBig) = _map[idx];

            if (spawnBig)
            {
                if (_bigSpawnedPerStage[idx] >= maxBig)
                    spawnBig = false;
            }

            return (max, spawnBig, idx);
        }
        
        private (float time, int max, bool spawnBig, int maxBig)[] InitMap() => new []
        {
            (0f, 0, false, 0),
            (8.654f, Min, false, 0),
            (24.976f, Max, false, 0),
            (60f, 0, false, 0),
            (60f + 25.061f, Max, true, BigMin),
            (60f + 41.853f, Min, false, 0),
            (60f + 57.088f, 0, false, 0),
            (120f + 3.4f, Min, false, 0),
            (120f + 28.733f, Max, true, BigMin),
            (120f + 45.638f, 0, false, 0),
            (120f + 49.606f, Max, true, BigMax),
            (180f + 4.728f, 0, false, 0),
            (180f + 15.568f, Min, false, 0),
            (180f + 23.034f, Max, true, BigMin),
            (180f + 41.261f, Max, true, BigMax),
            (180f + 54.489f, 0, false, 0)
        };
        
        private int Max => _gameplayManager.Parameters.EnemyMaxSquadSize;
        private int Min => _gameplayManager.Parameters.EnemyMinSquadSize;
        private int BigMax => _gameplayManager.Parameters.EnemyMaxBigUnits;
        private int BigMin => _gameplayManager.Parameters.EnemyMinBigUnits;
    }
}