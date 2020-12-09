using System;
using UnityEngine;

namespace Kugushev.Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Pool pool;
        [SerializeField] private float lifetimeSeconds = 3f;
        [SerializeField] private bool isEnemyBullet;
        [SerializeField] private float speed = 10f;
        [SerializeField] private FightColors fightColor;
        private float? _start;

        void Update()
        {
            if (_start == null) 
                _start = Time.time;

            if (Time.time - _start > lifetimeSeconds)
            {
                var currentGameObject = gameObject;
                currentGameObject.SetActive(false);
                _start = null;

                if (isEnemyBullet) 
                    pool.EnemyBulletsPool[fightColor].Enqueue(currentGameObject);
                else
                    pool.PlayerBulletsPool[fightColor].Enqueue(currentGameObject);
            }
        }

        private void FixedUpdate()
        {
            var t = transform;
            t.position += t.rotation * Vector3.forward * (Time.fixedDeltaTime * speed);
        }
    }
}
