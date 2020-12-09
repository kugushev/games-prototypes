using System.Collections.Generic;
using UnityEngine;

namespace Kugushev.Scripts
{
    [CreateAssetMenu]
    public class Pool : ScriptableObject
    {
        public Dictionary<FightColors, Queue<GameObject>> PlayerBulletsPool { get; } =
            new Dictionary<FightColors, Queue<GameObject>>
            {
                {FightColors.Red, new Queue<GameObject>()},
                {FightColors.Green, new Queue<GameObject>()},
                {FightColors.Blue, new Queue<GameObject>()},
                {FightColors.Yellow, new Queue<GameObject>()},
                {FightColors.Cyan, new Queue<GameObject>()},
                {FightColors.Magenta, new Queue<GameObject>()}
            };

        public Dictionary<FightColors, Queue<GameObject>> EnemyBulletsPool { get; } =
            new Dictionary<FightColors, Queue<GameObject>>
            {
                {FightColors.Red, new Queue<GameObject>()},
                {FightColors.Green, new Queue<GameObject>()},
                {FightColors.Blue, new Queue<GameObject>()},
                {FightColors.Yellow, new Queue<GameObject>()},
                {FightColors.Cyan, new Queue<GameObject>()},
                {FightColors.Magenta, new Queue<GameObject>()}
            };

        public Dictionary<FightColors, Queue<GameObject>> EnemiesPool { get; } =
            new Dictionary<FightColors, Queue<GameObject>>
            {
                {FightColors.Red, new Queue<GameObject>()},
                {FightColors.Green, new Queue<GameObject>()},
                {FightColors.Blue, new Queue<GameObject>()},
                {FightColors.Yellow, new Queue<GameObject>()},
                {FightColors.Cyan, new Queue<GameObject>()},
                {FightColors.Magenta, new Queue<GameObject>()}
            };
    }
}