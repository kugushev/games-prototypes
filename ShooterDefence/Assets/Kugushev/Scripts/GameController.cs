using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Kugushev.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private ColoredPrefabs enemyBulletPrefabs;
        [SerializeField] private ColoredPrefabs playerBulletPrefabs;
        [SerializeField] private ColoredPrefabs enemyPrefabs;
        [SerializeField] private Pool pool;

        public Vector3 PlayerPosition => player.transform.position;

        public GameObject GetNextEnemyBullet(FightColors color)
            => CreateOfTakeFromPool(pool.EnemyBulletsPool, enemyBulletPrefabs, color);

        public GameObject GetNextPlayerBullet(FightColors color) =>
            CreateOfTakeFromPool(pool.PlayerBulletsPool, playerBulletPrefabs, color);

        public GameObject GetNextEnemy(FightColors color) =>
            CreateOfTakeFromPool(pool.EnemiesPool, enemyPrefabs, color);

        private static GameObject CreateOfTakeFromPool(Dictionary<FightColors, Queue<GameObject>> pools,
            ColoredPrefabs prefabs, FightColors color)
        {
            var pool = pools[color];

            if (pool.Count > 0)
            {
                var bullet = pool.Dequeue();
                bullet.SetActive(true);
                return bullet;
            }

            var prefab = prefabs[color];

            return Instantiate(prefab);
        }
    }
}