using System;
using System.Diagnostics;
using UnityEngine;

namespace Kugushev.Scripts
{
    [CreateAssetMenu]
    public class ColoredPrefabs : ScriptableObject
    {
        [SerializeField] private GameObject redPrefab;
        [SerializeField] private GameObject greenPrefab;
        [SerializeField] private GameObject bluePrefab;
        [SerializeField] private GameObject yellowPrefab;
        [SerializeField] private GameObject cyanPrefab;
        [SerializeField] private GameObject magentaPrefab;

        public GameObject this[FightColors color]
        {
            get
            {
                switch (color)
                {
                    case FightColors.Red:
                        return redPrefab;
                    case FightColors.Green:
                        return greenPrefab;
                    case FightColors.Blue:
                        return bluePrefab;
                    case FightColors.Yellow:
                        return yellowPrefab;
                    case FightColors.Cyan:
                        return cyanPrefab;
                    case FightColors.Magenta:
                        return magentaPrefab;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(color), color, null);
                }
            }
        }
    }
}
