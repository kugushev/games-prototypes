using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float maxXOffset = 20f;
        [SerializeField] private float speed = 2f;
        [SerializeField] private ParticleSystem onDeathEffect;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Pool pool;
        [SerializeField] private FightColors fightColor;

        private GameController _gameController;
        private float movementDirection;
        private bool dying;

        private void Awake()
        {
            var obj = GameObject.FindWithTag("GameController");
            _gameController = obj.GetComponent<GameController>();
        }

        private void Start()
        {
            movementDirection = Random.Range(0, 2) == 0 ? 1 : -1;
            StartCoroutine(Shooting());
        }

        private void FixedUpdate()
        {
            var trans = transform;
            trans.position = Move(trans.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!dying && CompareMyColorTag(other)) 
                StartCoroutine(Dying());
        }

        private bool CompareMyColorTag(Collider other)
        {
            switch (fightColor)
            {
                case FightColors.Red:
                    return other.gameObject.CompareTag("PlayerBulletRed");
                case FightColors.Green:
                    return other.gameObject.CompareTag("PlayerBulletGreen");
                case FightColors.Blue:
                    return other.gameObject.CompareTag("PlayerBulletBlue");
                case FightColors.Yellow:
                    return other.gameObject.CompareTag("PlayerBulletYellow");
                case FightColors.Cyan:
                    return other.gameObject.CompareTag("PlayerBulletCyan");
                case FightColors.Magenta:
                    return other.gameObject.CompareTag("PlayerBulletMagenta");
                default:
                    Debug.LogError("Wrong color");
                    return false;
            }
        }

        private Vector3 Move(Vector3 position)
        {
            if (position.x > maxXOffset) 
                movementDirection = -1;
            if (position.x < -maxXOffset)
                movementDirection = 1;

            position.x += Time.fixedDeltaTime * speed * movementDirection;
            position.y = MovementFunction(position.x);

            return position;
        }

        private static float MovementFunction(float x) => Mathf.Sin(x);

        private IEnumerator Shooting()
        {
            var timeout = new WaitForSeconds(1f);
            var shortTimeout = new WaitForSeconds(0.1f);
            var bulletsInQueue = 0;
            var maxBulletsInQueue = 3;
            while (true)
            {
                var bullet = _gameController.GetNextEnemyBullet(fightColor);

                var bulletTransform = bullet.transform;
                bulletTransform.position = transform.position;
                bulletTransform.LookAt(_gameController.PlayerPosition);

                bulletsInQueue++;

                if (bulletsInQueue >= maxBulletsInQueue)
                {
                    bulletsInQueue = 0;
                    yield return timeout;
                }
                else
                    yield return shortTimeout;
            }
        }

        private IEnumerator Dying()
        {
            dying = true;

            meshRenderer.enabled = false;
            onDeathEffect.Emit(5);

            yield return new WaitForSeconds(0.5f);
            meshRenderer.enabled = true;

            GameObject currentGameObject = gameObject;
            currentGameObject.SetActive(false);
            pool.EnemiesPool[fightColor].Enqueue(currentGameObject);

            dying = false;
        }
    }
}