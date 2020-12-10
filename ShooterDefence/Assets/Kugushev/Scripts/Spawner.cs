using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        private readonly Vector2 _spawnArea = new Vector2(20f, 10f);

        private IEnumerator Start()
        {
            while (true)
            {
                var timeout = new WaitForSeconds(5f);
                
                var color = (FightColors) Random.Range((int) FightColors.Red, (int) FightColors.Magenta + 1);

                var enemy = gameController.GetNextEnemy(color);

                var position = enemy.transform.position;
                position.x = Random.Range(-_spawnArea.x, _spawnArea.x);
                position.y = Random.Range(-_spawnArea.y, _spawnArea.y);
                position.z = transform.position.z;
                enemy.transform.position = position;


                yield return timeout;
            }
        }
    }
}